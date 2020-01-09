﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.Office.Interop.Word;
using Services.Interfaces;
<<<<<<< HEAD
using System.Windows;
using Application = Microsoft.Office.Interop.Word.Application;
using Window = Microsoft.Office.Interop.Word.Window;
using System.IO;
using Services.Constants;
=======
>>>>>>> d2b9d017515476da329690706a1471df68883430

namespace Services
{
    public class WordLogicHandler : IWordLogicHandler
    {
<<<<<<< HEAD
        private Application _wordInstance;

        public void CreateNewLeaf(string path, string leafName, string treeName)
        {
            // Create word instance.
            Application wordInstance = CreateWordInstance();

            try
            {
                // Add document.
                object missing = Type.Missing;
                Document leaf = wordInstance.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                // Insert placeholder text in newly created document.
                InsertGenericTextOnLeaf(leafName, treeName, leaf);

                // Save and close leaf.
                leaf.SaveAs2(path);
                leaf.Close();
            }
            catch (COMException e)
            {
                throw new COMException(e.ToString());
            }
            finally
            {
                // Dispose word instance.
                wordInstance.Quit();
                GC.Collect();
            }
        }

        private Application CreateWordInstance()
        {
            var wordInstance = new Application();
            wordInstance.Visible = false;
            wordInstance.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            return wordInstance;
        }

        private void InsertGenericTextOnLeaf(string leafName, string treeName, Document leaf)
        {
            leaf.Range(0,0).Text = $"Welcome to your {leafName} Leaf in the {treeName} Tree! Delete this message and write anything you'd like. " +
            $"Do not forget to always hit the Save button when you are done writing. " +
            $"One last thing: try to always use the 'Save' button instead of 'Save As'. Just to make sure your leaf is in its proper tree.";
        }

        public void SaveAndCloseAllLeaves()
=======
        public IList<string> GetNamesOfAllOpenWordDocuments()
        {
            List<string> documentNames = new List<string>();

            try
            {
                Window objectWindow;
                Application wordInstance;
                wordInstance = (Application)Marshal.GetActiveObject("Word.Application");

                for (int i = 0; i <= wordInstance.Windows.Count; i++)
                {
                    object a = i + 1;
                    objectWindow = wordInstance.Windows.get_Item(ref a);
                    documentNames.Add(objectWindow.Document.FullName);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return documentNames;
        }

        public void SaveAndCloseAllOpenedWordDocuments()
>>>>>>> d2b9d017515476da329690706a1471df68883430
        {
            try
            {
                if (Process.GetProcessesByName("winword").Count() > 0)
                {
                    Application wordInstance = (Application)Marshal.GetActiveObject("Word.Application");

                    foreach (Document doc in wordInstance.Documents)
                    {
                        doc.Save();
                        doc.Close();
                    }

                    wordInstance.Quit();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

<<<<<<< HEAD
        public IList<string> GetAllOpenLeafNames()
        {
            List<string> documentNames = new List<string>();
            Window wordWindow;

            try
            {
                if (_wordInstance == null)
                {
                    _wordInstance = (Application)Marshal.GetActiveObject("Word.Application") ?? null;
                }

                // If there are any open windows (documents), get their names.
                if (_wordInstance.Windows.Count > 0)
                {
                    for (int i = 0; i < _wordInstance.Windows.Count; i++)
                    {
                        object a = i + 1;
                        wordWindow = _wordInstance.Windows.get_Item(ref a);
                        documentNames.Add(wordWindow.Document.FullName);
                    }
                }
            }
            catch (COMException ex)
            {
                _wordInstance = CreateWordInstance();
                return new List<string>();
                //throw new COMException(ex.Message);
            }

            return documentNames;
        }

        public void OpenExistingLeaf(string path)
        {
            IList<string> openedWordDocuments = GetAllOpenLeafNames();

            // Checks if current file is already open.
            foreach (string name in openedWordDocuments)
            {
                if (name.Equals(path))
                {
                    MessageBox.Show($"This leaf is already open, check your open word documents.", "Leaf Already Opened");
                    return;
                }
            }

            if (_wordInstance == null)
            {
                _wordInstance = (Application)Marshal.GetActiveObject("Word.Application");
            }

            try
            {
                _wordInstance.Visible = true;
                Document wordDocument = _wordInstance.Documents.Open(path);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CheckIfLeafIsOpen(string currentPath)
        {
            IList<string> openedWordDocumentsPaths = GetAllOpenLeafNames();

            // Checks if current file is already open.
            foreach (string documentPath in openedWordDocumentsPaths)
                if (documentPath.Equals(currentPath))
                    return true;

            return false;
        }

        public Dictionary<string, int> GetTotalTreeStatistics(string treeName)
        {
            // Get the object to hold the statistics we will acquire.
            var statistics = GetStatisticsContainer();

            // Gets names of all the leaves we will look through.
            var folderLogic = new FolderLogicHandler();
            var treePath = folderLogic.GetFullTreePath(treeName);
            var leavesInTree = folderLogic.GetAllLeafNamesWithNoExtension(treePath);

            // Open an instance of word to open the documents in.
            Application application = new Application();

            foreach (string leaf in leavesInTree)
            {
                string fullLeafPath = folderLogic.GetFullLeafPath(treeName, leaf);
                Document document = application.Documents.Open(fullLeafPath);

                // Prepare to get statistics.
                object includeFootnotesAndEndnotes = true;
                WdStatistic wordStats = WdStatistic.wdStatisticWords;
                WdStatistic charStats = WdStatistic.wdStatisticCharacters;

                // Update statistics.
                statistics[StatsNamingConstants.WordCount] += document.ComputeStatistics(wordStats, ref includeFootnotesAndEndnotes);
                statistics[StatsNamingConstants.CharacterCount] += document.ComputeStatistics(charStats, ref includeFootnotesAndEndnotes);
                statistics[StatsNamingConstants.LeafCount]++;

                document.Close();
            }

            return statistics;
        }

        private static Dictionary<string, int> GetStatisticsContainer()
        {
            var output = new Dictionary<string, int>();
            output.Add(StatsNamingConstants.WordCount, 0);
            output.Add(StatsNamingConstants.LeafCount, 0);
            output.Add(StatsNamingConstants.CharacterCount, 0);
            return output;
=======
        public Application CreateWordDocumentFromExistingWordInstance(string path)
        {
            try
            {
                Application wordApp = (Application)Marshal.GetActiveObject("Word.Application");
                object inputFile = path;
                object confirmConversions = false;
                object readOnly = false;
                object visible = true;
                object missing = Type.Missing;

                Document doc = wordApp.Documents.Open(
                    ref inputFile, ref confirmConversions, ref readOnly, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref visible,
                    ref missing, ref missing, ref missing, ref missing);
                return wordApp;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
>>>>>>> d2b9d017515476da329690706a1471df68883430
        }
    }
}