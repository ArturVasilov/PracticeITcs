﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentsSecurity
{
    public partial class DocumentsForm : Form
    {
        private Company company;

        public DocumentsForm()
        {
            InitializeComponent();
            company = Company.Instance;
            //add all documents to list
        }

        private void AddDocumentButton_Click(object sender, EventArgs e)
        {
            AddDocumentDialog addDocumentDialog = new AddDocumentDialog();

            if (addDocumentDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                Document document = addDocumentDialog.getDocument;
                company.addDocument(document);
                //add document to form list
            }
        }

        private void AddProgrammerDocumentButton_Click(object sender, EventArgs e)
        {
            AddProgrammerDialog addProgrammerDialog = new AddProgrammerDialog();

            if (addProgrammerDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                Programmer programmer = addProgrammerDialog.getProgrammer;
                company.addProgrammer(programmer);
                //add document to form list
            }
        }

        private void AddProjectDocumentButton_Click(object sender, EventArgs e)
        {
            AddProjectDialog addProjectDialog = new AddProjectDialog();

            if (addProjectDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                Project project = addProjectDialog.getProject;
                company.addProject(project);
            }
        }

        private void AddFinanceDocumentButton_Click(object sender, EventArgs e)
        {
            AddFinanceDialog addFinanceDialog = new AddFinanceDialog();

            if (addFinanceDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                Finance finance = addFinanceDialog.getFinance;
                company.addFinance(finance);
            }
        }
    }
}
