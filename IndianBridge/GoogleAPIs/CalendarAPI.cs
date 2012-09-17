
using System;
using Google.GData.Client;
using Google.GData.Calendar;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace IndianBridge.GoogleAPIs
{

    public class IndianCalendarEvent
    {
        private String m_title; 
        private DateTime m_startDate;
        private String m_location;

        private int m_rowNumber;

        public IndianCalendarEvent(DateTime startDate, String title, String location, int rowNumber)
        {
            m_location = location;
            m_startDate = startDate;
            m_title = title;
            m_rowNumber = rowNumber;
        }
        public String Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public DateTime StartDate
        {
            get { return m_startDate; }
            set { m_startDate = value; }
        }
        public String Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        public int RowNumber
        {
            get { return m_rowNumber; }
            set { m_rowNumber = value; }
        }

    }
    public class CalendarAPI
    {
        static public String APP_NAME = "CalendarAPI-v0.1";
        private CalendarService m_service = null;
        private AtomEntryCollection m_entries = null;
        public static Tuple<Boolean,Boolean> checkCredentials(String scope, String username, String password) {
            EventEntry entry;
            try
            {
                CalendarService service = new CalendarService(APP_NAME);
                service.setUserCredentials(username, password);

                EventQuery query = new EventQuery(scope);
                query.StartTime = DateTime.Parse("12/31/1979");
                query.EndTime = DateTime.Parse("1/2/1981");
                query.Query = "Authentication";
                query.NumberToRetrieve = 1;
                EventFeed calFeed = service.Query(query);

                if (calFeed.Entries.Count != 1)
                {
                    throw new Exception("Unable to find Authentication Event!!!");
                }
                entry = (EventEntry)calFeed.Entries[0];
                entry.Content.Content = DateTime.Now.ToString();
                //entry.Locations[0].ValueString = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return new Tuple<Boolean,Boolean>(false,false);
            }
            try {
                entry.Update();
                return new Tuple<Boolean, Boolean>(true, true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return new Tuple<Boolean, Boolean>(true, false);
            }
        }
        private String removeOldResultsText_(String text) {
            String patternString = @"<a([^>]+)>\s*(results|result)\s*</a>";
            String newText = text;
            newText = Regex.Replace(newText, patternString, "", RegexOptions.IgnoreCase);
            patternString = @"(result|results)\s*:\s*([^\s\n\t\f\v\0]*)";
            newText = Regex.Replace(newText, patternString, "", RegexOptions.IgnoreCase);
            return newText;
        }

        public Boolean updateResults(int entryNumber, String resultsURL)
        {
            try
            {
                EventEntry entry = (EventEntry)m_entries[entryNumber];
                String contents = entry.Content.Content;
                entry.Content.Content = "<a href=\"" + resultsURL + "\">Results</a>\n" + removeOldResultsText_(contents);
                entry.Update();
                EventQuery query = new EventQuery("https://www.google.com/calendar/feeds/dl2ahhopqnt5j8p0082h1pst3s%40group.calendar.google.com/private/full");
                query.StartTime = entry.Times[0].StartTime;
                query.EndTime = entry.Times[0].EndTime;
                query.Query = entry.Title.Text;
                query.NumberToRetrieve = 1;
                EventFeed calFeed = m_service.Query(query);

                if (calFeed.Entries.Count >= 1)
                {
                    entry = (EventEntry)calFeed.Entries[0];
                    contents = entry.Content.Content;
                    entry.Content.Content = "<a href=\"" + resultsURL + "\">Results</a>\n" + removeOldResultsText_(contents);
                    entry.Update();
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public CalendarAPI(String username, String password)
        {
            m_service = new CalendarService(APP_NAME);
            // Set your credentials
            m_service.setUserCredentials(username, password);
            
        }
        public SortableBindingList<IndianCalendarEvent> getEvents(DateTime startDate, DateTime endDate, String searchString)
        {
            SortableBindingList<IndianCalendarEvent> events = new SortableBindingList<IndianCalendarEvent>();
            Trace.WriteLine("Getting Events");
            EventQuery query = new EventQuery("http://www.google.com/calendar/feeds/indianbridge@gmail.com/private/full");
            query.SingleEvents = true;
            query.Query = searchString;
            query.StartTime = startDate;
            query.EndTime = endDate;

            // Tell the service to query:
            EventFeed calFeed = m_service.Query(query) as EventFeed;
            m_entries = calFeed.Entries;
            for (int i = 0; i < calFeed.Entries.Count; ++i)
            {
                
                EventEntry entry = (EventEntry)calFeed.Entries[i];
                Trace.WriteLine(entry.Title.Text + " at position " + i);
                IndianCalendarEvent ice = new IndianCalendarEvent(entry.Times[0].StartTime, entry.Title.Text, entry.Locations[0].ValueString, i);
                events.Add(ice);
            }
            Trace.WriteLine("Found " + calFeed.Entries.Count + " items");
            return events;
        }
     }
    /// <summary>
    /// Provides a generic collection that supports data binding and additionally supports sorting.
    /// See http://msdn.microsoft.com/en-us/library/ms993236.aspx
    /// If the elements are IComparable it uses that; otherwise compares the ToString()
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SortableBindingList<T> : BindingList<T> where T : class
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor _sortProperty;

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return _isSorted; }
        }

        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirection; }
        }

        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortProperty; }
        }

        /// <summary>
        /// Removes any sort applied with ApplySortCore if sorting is implemented
        /// </summary>
        protected override void RemoveSortCore()
        {
            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;
        }

        /// <summary>
        /// Sorts the items if overridden in a derived class
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            _sortProperty = prop;
            _sortDirection = direction;

            List<T> list = Items as List<T>;
            if (list == null) return;

            list.Sort(Compare);

            _isSorted = true;
            //fire an event that the list has been changed.
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }


        private int Compare(T lhs, T rhs)
        {
            var result = OnComparison(lhs, rhs);
            //invert if descending
            if (_sortDirection == ListSortDirection.Descending)
                result = -result;
            return result;
        }

        private int OnComparison(T lhs, T rhs)
        {
            object lhsValue = lhs == null ? null : _sortProperty.GetValue(lhs);
            object rhsValue = rhs == null ? null : _sortProperty.GetValue(rhs);
            if (lhsValue == null)
            {
                return (rhsValue == null) ? 0 : -1; //nulls are equal
            }
            if (rhsValue == null)
            {
                return 1; //first has value, second doesn't
            }
            if (lhsValue is IComparable)
            {
                return ((IComparable)lhsValue).CompareTo(rhsValue);
            }
            if (lhsValue.Equals(rhsValue))
            {
                return 0; //both are the same
            }
            //not comparable, compare ToString
            return lhsValue.ToString().CompareTo(rhsValue.ToString());
        }
    }
}
