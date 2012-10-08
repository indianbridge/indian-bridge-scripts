using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Documents;
using System.IO;
using Google.GData.Client;
using Google.GData.Client.ResumableUpload;
using System.ComponentModel;

namespace IndianBridge.GoogleAPIs
{
    public class DocsAPI
    {
        static public string APP_NAME = "DocsAPI-v0.1";
        private DocumentsService m_service = null;
        private string m_username = "";
        private string m_password = "";
        private BackgroundWorker m_worker;
        private DoWorkEventArgs m_e;
        private bool m_runningInBackground = false;
        private int totalPagesToUpload = 0;
        private int numberOfPagesAlreadyUploaded = 0;
        private string m_prefixString = "";
        private Dictionary<string,string> m_fileFilters = null;
        public List<string> listOfFolders = new List<string>();

        public DocsAPI( string username, string password)
        {
            m_service = new DocumentsService(APP_NAME);
            m_username = username;
            m_password = password;
            m_service.setUserCredentials(username, password);
        }

        public void getListOfFolders(string downloadFrom)
        {
            getListOfFoldersInternal(downloadFrom);
        }

        public void getListOfFoldersInBackground(object sender, DoWorkEventArgs e)
        {
            string downloadFrom = e.Argument as string;
            getListOfFoldersInternal(downloadFrom);
        }

        private void getListOfFoldersInternal(string downloadFrom)
        {
            listOfFolders = new List<string>();
            DocumentEntry parentFolder = locateFolder(downloadFrom, null);
            if (!string.IsNullOrWhiteSpace(downloadFrom) && parentFolder == null) return;
            FolderQuery contentQuery = new FolderQuery(parentFolder.ResourceId);
            contentQuery.ShowFolders = true;
            DocumentsFeed contents = m_service.Query(contentQuery);
            foreach (DocumentEntry entry in contents.Entries)
            {
                if (entry.IsFolder) listOfFolders.Add(entry.Title.Text);
            }
        }

        public void DownloadGoogleDocsToDirectoryInBackground(object sender, DoWorkEventArgs e)
        {
            Tuple<string, string, Dictionary<string, string>> values = (Tuple<string, string, Dictionary<string, string>>)e.Argument;
            string downloadFrom = values.Item1;
            string downloadTo = values.Item2;
            Dictionary<string, string> fileFilters = values.Item3;
            m_runningInBackground = true;
            m_worker = sender as BackgroundWorker;
            m_e = e;
            downloadGoogleDocsToDirectoryInternal(downloadFrom, downloadTo, fileFilters);
            if (cancel()) m_e.Cancel = true;
        }

        public void downloadGoogleDocsToDirectory(string downloadFrom,string downloadTo, Dictionary<string, string> fileFilters)
        {
            m_runningInBackground = false;
            m_worker = null;
            m_e = null;
            downloadGoogleDocsToDirectoryInternal(downloadFrom, downloadTo, fileFilters);
            if (cancel()) m_e.Cancel = true;
        }

        private void downloadGoogleDocsToDirectoryInternal(string downloadFrom, string downloadTo, Dictionary<string, string> fileFilters)
        {
            m_fileFilters = fileFilters;
            if (!Directory.Exists(downloadTo)) Directory.CreateDirectory(downloadTo);
            totalPagesToUpload = countFoldersAndFiles(downloadFrom);
            if (cancel()) return;
            downloadDirectory(downloadFrom,downloadTo);
        }

        private void downloadDirectory(string downloadFrom, string downloadTo)
        {
            DocumentEntry parentFolder = locateFolder(downloadFrom, null);
            if (cancel()) return;
            if (!string.IsNullOrWhiteSpace(downloadFrom) && parentFolder == null) return;
            downloadDirectory(parentFolder, downloadTo);
        }

        private void downloadDirectory(DocumentEntry folder, string downloadTo)
        {
            if (cancel()) return;
            string currentFolderName = Path.Combine(downloadTo, folder.Title.Text);
            Directory.CreateDirectory(currentFolderName);
            reportProgress("Dowloaded/Updated " + folder.Title.Text);
            FolderQuery contentQuery = new FolderQuery(folder.ResourceId);
            contentQuery.ShowFolders = true;
            DocumentsFeed contents = m_service.Query(contentQuery);
            foreach (DocumentEntry entry in contents.Entries)
            {
                if (cancel()) return;
                if (!entry.IsFolder)
                {
                    if (m_fileFilters.ContainsKey(Path.GetExtension(entry.Title.Text)))
                    {
                        string downloadUrl = entry.Content.Src.Content;
                        if (cancel()) return;
                        Stream stream = m_service.Query(new Uri(downloadUrl));
                        if (cancel()) return;
                        string fileName = Path.Combine(currentFolderName, entry.Title.Text);
                        FileStream outputFileStream = new FileStream(fileName, FileMode.Create);
                        stream.CopyTo(outputFileStream);
                        outputFileStream.Close();
                        reportProgress("Downloaded/Updated " + entry.Title.Text);
                        if (cancel()) return;
                    }
                }
                else
                {
                    downloadDirectory(entry, currentFolderName);
                }
            }
        }

        private int countFoldersAndFiles(string downloadFrom)
        {
            DocumentEntry parentFolder = locateFolder(downloadFrom, null);
            if (cancel()) return 0;
            if (!string.IsNullOrWhiteSpace(downloadFrom) && parentFolder == null) return 0;
            return countFoldersAndFiles(parentFolder);
        }

        private int countFoldersAndFiles(DocumentEntry folder)
        {
            if (cancel()) return 0;
            int count = 1;
            FolderQuery contentQuery = new FolderQuery(folder.ResourceId);
            contentQuery.ShowFolders = true;
            DocumentsFeed contents = m_service.Query(contentQuery);
            foreach (DocumentEntry entry in contents.Entries)
            {
                if (cancel()) return 0;
                if (!entry.IsFolder)
                {
                    if (m_fileFilters.ContainsKey(Path.GetExtension(entry.Title.Text))) count++;
                }
                else
                {
                    count += countFoldersAndFiles(entry);
                }
            }
            return count;
        }

        public void uploadDirectoryToGoogleDocsInBackground(object sender, DoWorkEventArgs e)
        {
            Tuple<string, string, Dictionary<string, string>> values = (Tuple<string, string, Dictionary<string, string>>)e.Argument;
            string directory = values.Item1;
            string uploadTo = values.Item2;
            Dictionary<string, string> fileFilters = values.Item3;
            m_runningInBackground = true;
            m_worker = sender as BackgroundWorker;
            m_e = e;
            uploadDirectoryInternal(directory, uploadTo,fileFilters);
            if (cancel()) m_e.Cancel = true;
        }

        public void uploadDirectoryToGoogleDocs(String directory, string uploadTo, Dictionary<string, string> fileFilters)
        {
            m_runningInBackground = false;
            m_worker = null;
            m_e = null;
            uploadDirectoryInternal(directory,uploadTo,fileFilters);
            if (cancel()) m_e.Cancel = true;
        }

        private void uploadDirectoryInternal(string directory, string uploadTo, Dictionary<string, string> fileFilters)
        {
            m_fileFilters = fileFilters;
            if (!Directory.Exists(directory))
                throw new System.ArgumentException("Only a directory structure can be uploaded");
            DirectoryInfo dir = new DirectoryInfo(directory);
            totalPagesToUpload = 1 + dir.GetDirectories("*", SearchOption.AllDirectories).Length;
            foreach (string fileFilter in m_fileFilters.Keys) totalPagesToUpload += dir.GetFiles(fileFilter, SearchOption.AllDirectories).Length;
            DocumentEntry parentFolder = locateFolder(uploadTo, null);
            if (cancel()) return;
            if (!string.IsNullOrWhiteSpace(uploadTo) && parentFolder == null) return;
            uploadDirectory(directory,parentFolder);
        }

        private DocumentEntry locateFolder(string path, DocumentEntry parentFolder=null)
        {
            string[] directories = path.Split(Path.DirectorySeparatorChar);
            foreach (string directory in directories)
            {
                if (cancel()) return null;
                parentFolder = findFolder(directory, parentFolder);
                if (parentFolder == null)
                {
                    IndianBridge.Common.Utilities.showErrorMessage("Cannot find Folder "+directory+" in Google Collections List");
                    return null;
                }
            }
            return parentFolder;
        }

        private void uploadDirectory(string directory, DocumentEntry parentFolder = null)
        {
            if (cancel()) return;
            DocumentEntry currentFolder = createFolder(Path.GetFileName(directory), parentFolder);
            reportProgress(Path.GetFileName(directory));
            // Process all sub folders
            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                if (cancel()) return;
                uploadDirectory(subDirectory, currentFolder);
            }
            foreach (string file in Directory.GetFiles(directory,"*.ini",SearchOption.TopDirectoryOnly)) {
                if (cancel()) return;
                uploadFile(file, currentFolder);
                reportProgress(Path.GetFileName(file));
            }
            foreach (string file in Directory.GetFiles(directory, "*.mdb", SearchOption.TopDirectoryOnly))
            {
                if (cancel()) return;
                uploadFile(file, currentFolder);
                reportProgress(Path.GetFileName(file));
            }
        }

        private bool cancel()
        {
            return m_runningInBackground && m_worker.CancellationPending;
        }

        private void reportProgress(string title)
        {
            if (m_runningInBackground)
            {
                numberOfPagesAlreadyUploaded++;
                double percentage = ((double)numberOfPagesAlreadyUploaded / (double)totalPagesToUpload) * 100;
                if (percentage > 100) percentage = 100;
                m_worker.ReportProgress(Convert.ToInt32(percentage), m_prefixString + title);
            }
        }

        private DocumentEntry findFolder(string folderName,DocumentEntry parentFolder=null) {
            if (cancel()) return null;
            FolderQuery query = (parentFolder == null ? new FolderQuery() : new FolderQuery(parentFolder.ResourceId));
            query.TitleExact = true;
            query.Title = folderName;
            DocumentsFeed feed = m_service.Query(query);
            if (feed.Entries.Count == 0) return null;
            return (DocumentEntry)feed.Entries[0];
        }

        private DocumentEntry createFolder(string folderName, DocumentEntry parentFolder)
        {
            if (cancel()) return null;
            DocumentEntry currentFolder = findFolder(folderName, parentFolder);
            if (cancel()) return null;
            if (currentFolder != null) return currentFolder;
            FolderQuery contentQuery = new FolderQuery(parentFolder.ResourceId);
            DocumentEntry subfolder = new DocumentEntry();
            subfolder.IsFolder = true;
            subfolder.Title.Text = folderName;
            if (cancel()) return null;
            DocumentEntry newFolder = m_service.Insert(contentQuery.Uri, subfolder);
            return newFolder;
        }



        public void uploadFile(string fileName, DocumentEntry parentFolder)
        {
            if (cancel()) return;
            string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
            if(!m_fileFilters.ContainsKey(fileExtension)) return;
            int CHUNK_SIZE = 1;
            ResumableUploader ru = new ResumableUploader(CHUNK_SIZE);
            ru.AsyncOperationCompleted += new AsyncOperationCompletedEventHandler(this.OnDone);
            ru.AsyncOperationProgress += new AsyncOperationProgressEventHandler(this.OnProgress);

            // Check if entry exists 
            FolderQuery contentQuery = new FolderQuery(parentFolder.ResourceId);
            contentQuery.Title = Path.GetFileName(fileName);
            contentQuery.TitleExact = true;
            DocumentsFeed contents = m_service.Query(contentQuery);
            bool fileExists = contents.Entries.Count > 0;
            DocumentEntry entry = fileExists?contents.Entries[0] as DocumentEntry:new DocumentEntry();
            entry.Title.Text = Path.GetFileName(fileName);
            string mimeType = m_fileFilters[fileExtension];
            entry.MediaSource = new MediaFileSource(fileName,mimeType);
            // Define the resumable upload link
            string notConvert = "?convert=false";
            Uri createUploadUrl;
            if (parentFolder == null) createUploadUrl = new Uri("https://docs.google.com/feeds/upload/create-session/default/private/full" + notConvert);
            else createUploadUrl = new Uri("https://docs.google.com/feeds/upload/create-session/default/private/full/" + parentFolder.ResourceId + "/contents" + notConvert);
            AtomLink link = new AtomLink(createUploadUrl.AbsoluteUri);
            link.Rel = ResumableUploader.CreateMediaRelation;
            entry.Links.Add(link);

            // Set the service to be used to parse the returned entry
            entry.Service = m_service;

            // Instantiate the ResumableUploader component.
            ResumableUploader uploader = new ResumableUploader();

            // Set the handlers for the completion and progress events
            uploader.AsyncOperationCompleted += new AsyncOperationCompletedEventHandler(OnDone);
            uploader.AsyncOperationProgress += new AsyncOperationProgressEventHandler(OnProgress);
            ClientLoginAuthenticator cla = new ClientLoginAuthenticator("uploader", ServiceNames.Documents, m_username, m_password);
            // Start the upload process
            if (cancel()) return;
            if (fileExists) uploader.UpdateAsync(cla, entry, new Object());
            else uploader.InsertAsync(cla, entry, new Object());
        }
        
        private void OnDone(object sender, AsyncOperationCompletedEventArgs e) {
            DocumentEntry entry = e.Entry as DocumentEntry;
            //Console.WriteLine("Error = " + e.Error.Message);
            //IndianBridge.Common.Utilities.showWarningessage("Uploaded " + entry.Title.Text);
        }
        
        private void OnProgress(object sender, AsyncOperationProgressEventArgs e) {
            int percentage = e.ProgressPercentage;
        } 
    }
}
