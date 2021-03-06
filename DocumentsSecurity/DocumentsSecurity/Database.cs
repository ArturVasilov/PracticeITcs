﻿using System;
using System.Collections.Generic;

using System.Data;
using System.Text;

namespace DocumentsSecurity
{
    internal class Database
    {
        internal const string FILENAME = "alldocuments.xml";

        private DataSet database;

        public Database()
        {
            loadDatabase();
        }

        internal void loadDatabase()
        {
            database = new DataSet();
            try
            {
                database.ReadXml(FILENAME, XmlReadMode.ReadSchema);
                DatabaseConstants.IdsKeeper.init();
                //throw new Exception();
            } catch (Exception) 
            {
                createNewDatabase();
            }
        }

        #region add documents functions
        internal void addDocument(Report report)
        {
            if (report == null)
            {
                throw new ArgumentNullException("tried to add null reference for a report document");
            }
            DataTable table = database.Tables[DatabaseConstants.Report.TABLE_NAME];
            DataRow row = table.NewRow();
            row[DatabaseConstants.Report.ID] = report.Id;
            row[DatabaseConstants.Report.AUTHOR] = report.AuthorId;
            row[DatabaseConstants.Report.CONTENT] = report.Description;
            table.Rows.Add(row);
            DatabaseConstants.IdsKeeper.addReport();
        }

        internal void addDocument(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("tried to add null reference for a project document");
            }

            DataTable projectsTable = database.Tables[DatabaseConstants.Project.TABLE_NAME];
            DataRow projectRow = projectsTable.NewRow();
            projectRow[DatabaseConstants.Project.ID] = project.Id;
            projectRow[DatabaseConstants.Project.CUSTOMER] = project.Customer;
            projectRow[DatabaseConstants.Project.COST] = project.Cost;
            projectRow[DatabaseConstants.Project.DATE] = project.Date;
            projectRow[DatabaseConstants.Project.DESCRIPTION] = project.Description;
            projectsTable.Rows.Add(projectRow);

            DataTable performersTable = database.Tables[DatabaseConstants.Performer.TABLE_NAME];
            foreach (int performerId in project.PerformersIds)
            {
                DataRow performerRow = performersTable.NewRow();
                performerRow[DatabaseConstants.Performer.PROJECT_ID] = project.Id;
                performerRow[DatabaseConstants.Performer.PROGRAMMER_ID] = performerId;
                performersTable.Rows.Add(performerRow);
            }
            DatabaseConstants.IdsKeeper.addProject();
        }

        internal void addDocument(Programmer programmer)
        {
            if (programmer == null)
            {
                throw new ArgumentNullException("tried to add null reference for a programmer document");
            }
            DataTable programmersTable = database.Tables[DatabaseConstants.Programmer.TABLE_NAME];
            DataRow programmerRow = programmersTable.NewRow();
            programmerRow[DatabaseConstants.Programmer.ID] = programmer.Id;
            programmerRow[DatabaseConstants.Programmer.NAME] = programmer.Name;
            programmerRow[DatabaseConstants.Programmer.SALARY] = programmer.Salary;
            programmerRow[DatabaseConstants.Programmer.DESCRIPTION] = programmer.Description;

            DataTable programmersSkillsTable = database.Tables[DatabaseConstants.ProgrammersSkills.TABLE_NAME];
            foreach (int skillId in programmer.SkillsIds)
            {
                DataRow skillRow = programmersSkillsTable.NewRow();
                skillRow[DatabaseConstants.ProgrammersSkills.PROGRAMMER_ID] = programmer.Id;
                skillRow[DatabaseConstants.ProgrammersSkills.SKILL_ID] = skillId;
                programmersSkillsTable.Rows.Add(skillRow);
            }

            programmersTable.Rows.Add(programmerRow);
            DatabaseConstants.IdsKeeper.addProgrammer();
        }

        internal void addDocument(Finance finance)
        {
            if (finance == null)
            {
                throw new ArgumentNullException("tried to add null reference for a finance document");
            }
            DataTable financeTable = database.Tables[DatabaseConstants.Finance.TABLE_NAME];
            DataRow financeRow = financeTable.NewRow();
            financeRow[DatabaseConstants.Finance.ID] = finance.Id;
            financeRow[DatabaseConstants.Finance.INCOME] = finance.Income;
            financeRow[DatabaseConstants.Finance.EXPENSE] = finance.Expense;
            financeRow[DatabaseConstants.Finance.PROFIT] = finance.Profit;
            financeRow[DatabaseConstants.Finance.DESCRIPTION] = finance.Description;
            financeTable.Rows.Add(financeRow);
            DatabaseConstants.IdsKeeper.addFinance();
        }
        #endregion

        #region getters for documents
        internal Programmer getProgrammerById(int id)
        {
            DataTable table = database.Tables[DatabaseConstants.Programmer.TABLE_NAME];
            DataRow row = table.Rows.Find(id);
            string description = row[DatabaseConstants.Programmer.DESCRIPTION].ToString();
            string name = row[DatabaseConstants.Programmer.NAME].ToString();
            int salary = (int)row[DatabaseConstants.Programmer.SALARY];

            table = database.Tables[DatabaseConstants.ProgrammersSkills.TABLE_NAME];
            List<int> ids = new List<int>();
            foreach (DataRow dataRow in table.Select(DatabaseConstants.ProgrammersSkills.PROGRAMMER_ID + "=" + id))
            {
                ids.Add((int)dataRow[DatabaseConstants.ProgrammersSkills.SKILL_ID]);
            }
            int[] skillsIds = ids.ToArray();

            return new Programmer(id, description, name, salary, skillsIds);
        }

        internal Project getProjectById(int id)
        {
            DataTable table = database.Tables[DatabaseConstants.Project.TABLE_NAME];
            DataRow row = table.Rows.Find(id);
            string description = row[DatabaseConstants.Project.DESCRIPTION].ToString();
            string customer = row[DatabaseConstants.Project.CUSTOMER].ToString();
            long cost = (long)row[DatabaseConstants.Project.COST];
            string date = row[DatabaseConstants.Project.DATE].ToString();

            table = database.Tables[DatabaseConstants.Performer.TABLE_NAME];
            List<int> ids = new List<int>();
            foreach (DataRow dataRow in table.Select(DatabaseConstants.Performer.PROJECT_ID + "=" + id))
            {

                ids.Add((int)dataRow[DatabaseConstants.Performer.PROGRAMMER_ID]);
            }
            int[] performersIds = ids.ToArray();

            return new Project(id, description, customer, cost, date, performersIds);
        }

        internal Finance getFinanceById(int id)
        {
            DataTable table = database.Tables[DatabaseConstants.Finance.TABLE_NAME];
            DataRow row = table.Rows.Find(id);
            return new Finance(id, row[DatabaseConstants.Finance.DESCRIPTION].ToString(),
                (long)row[DatabaseConstants.Finance.INCOME],
                (long)row[DatabaseConstants.Finance.EXPENSE]);
        }

        internal Report getReportById(int id)
        {
            DataTable table = database.Tables[DatabaseConstants.Report.TABLE_NAME];
            DataRow row = table.Rows.Find(id);
            return new Report(id, (int)row[DatabaseConstants.Report.AUTHOR], 
                row[DatabaseConstants.Report.CONTENT].ToString());
        }

        internal List<Programmer.NameIdPair> getAllProgrammers
        {
            get
            {
                List<Programmer.NameIdPair> programmers = new List<Programmer.NameIdPair>();
                foreach (DataRow row in database.Tables[DatabaseConstants.Programmer.TABLE_NAME].Rows)
                {
                    int id = (int)row[DatabaseConstants.Programmer.ID];
                    string name = row[DatabaseConstants.Programmer.NAME].ToString();
                    programmers.Add(new Programmer.NameIdPair(id, name));
                }
                return programmers;
            }
        }

        public List<TableNameIdValueTriple> AllDocuments
        {
            get
            {
                List<TableNameIdValueTriple> list = new List<TableNameIdValueTriple>();

                string tableName = DatabaseConstants.Project.TABLE_NAME;
                string value = "Заказ от ";
                DataTable table = database.Tables[tableName];
                foreach (DataRow row in table.Rows)
                {
                    int id = (int)row[DatabaseConstants.Project.ID];
                    string addValue = row[DatabaseConstants.Project.CUSTOMER].ToString();
                    list.Add(new TableNameIdValueTriple(id, tableName, value + addValue));
                }

                tableName = DatabaseConstants.Programmer.TABLE_NAME;
                table = database.Tables[tableName];
                value = "Программист ";
                foreach (DataRow row in table.Rows)
                {
                    int id = (int)row[DatabaseConstants.Programmer.ID];
                    string addValue = row[DatabaseConstants.Programmer.NAME].ToString();
                    list.Add(new TableNameIdValueTriple(id, tableName, value + addValue));
                }

                tableName = DatabaseConstants.Finance.TABLE_NAME;
                table = database.Tables[tableName];
                value = "Прибыль: ";
                foreach (DataRow row in table.Rows)
                {
                    int id = (int)row[DatabaseConstants.Finance.ID];
                    string addValue = row[DatabaseConstants.Finance.PROFIT].ToString();
                    list.Add(new TableNameIdValueTriple(id, tableName, value + addValue));
                }

                tableName = DatabaseConstants.Report.TABLE_NAME;
                table = database.Tables[tableName];
                value = "Отчет номер ";
                foreach (DataRow row in table.Rows)
                {
                    int id = (int)row[DatabaseConstants.Report.ID];
                    list.Add(new TableNameIdValueTriple(id, tableName, value + id));
                }

                return list;
            }
        }
        #endregion

        #region edit functions
        internal void editProgrammer(Programmer programmer)
        {
            DataRow programmerRow = database.Tables[DatabaseConstants.Programmer.TABLE_NAME].Rows.Find(programmer.Id);
            programmerRow[DatabaseConstants.Programmer.NAME] = programmer.Name;
            programmerRow[DatabaseConstants.Programmer.SALARY] = programmer.Salary;
            programmerRow[DatabaseConstants.Programmer.DESCRIPTION] = programmer.Description;

            DataTable skillsTable = database.Tables[DatabaseConstants.ProgrammersSkills.TABLE_NAME];
            foreach (DataRow row in skillsTable.Select(DatabaseConstants.ProgrammersSkills.PROGRAMMER_ID + "=" + programmer.Id))
            {
                skillsTable.Rows.Remove(row);
            }
            foreach (int skillId in programmer.SkillsIds)
            {
                DataRow row = skillsTable.NewRow();
                row[DatabaseConstants.ProgrammersSkills.PROGRAMMER_ID] = programmer.Id;
                row[DatabaseConstants.ProgrammersSkills.SKILL_ID] = skillId;
                skillsTable.Rows.Add(row);
            }
        }

        internal void editProject(Project project)
        {
            DataRow projectRow = database.Tables[DatabaseConstants.Project.TABLE_NAME].Rows.Find(project.Id);
            projectRow[DatabaseConstants.Project.CUSTOMER] = project.Customer;
            projectRow[DatabaseConstants.Project.COST] = project.Cost;
            projectRow[DatabaseConstants.Project.DATE] = project.Date;
            projectRow[DatabaseConstants.Project.DESCRIPTION] = project.Description;

            DataTable performersTable = database.Tables[DatabaseConstants.Performer.TABLE_NAME];
            foreach (DataRow row in performersTable.Select(DatabaseConstants.Performer.PROJECT_ID + "=" + project.Id))
            {
                performersTable.Rows.Remove(row);
            }
            foreach (int performerId in project.PerformersIds)
            {
                DataRow row = performersTable.NewRow();
                row[DatabaseConstants.Performer.PROJECT_ID] = project.Id;
                row[DatabaseConstants.Performer.PROGRAMMER_ID] = performerId;
                performersTable.Rows.Add(row);
            }
        }

        internal void editFinance(Finance finance)
        {
            DataRow row = database.Tables[DatabaseConstants.Finance.TABLE_NAME].Rows.Find(finance.Id); 
            row[DatabaseConstants.Finance.INCOME] = finance.Income;
            row[DatabaseConstants.Finance.EXPENSE] = finance.Expense;
            row[DatabaseConstants.Finance.PROFIT] = finance.Profit;
            row[DatabaseConstants.Finance.DESCRIPTION] = finance.Description;
        }

        internal void editReport(Report report)
        {
            DataRow row = database.Tables[DatabaseConstants.Report.TABLE_NAME].Rows.Find(report.Id);
            row[DatabaseConstants.Report.AUTHOR] = report.AuthorId;
            row[DatabaseConstants.Report.CONTENT] = report.Description;
        }
        #endregion

        #region remove functions
        internal void remove(string tableName, int id)
        {
            switch (tableName)
            {
                case DatabaseConstants.Programmer.TABLE_NAME:
                    removeProgrammer(id);
                    break;

                case DatabaseConstants.Project.TABLE_NAME:
                    removeProject(id);
                    break;

                case DatabaseConstants.Report.TABLE_NAME:
                    DataTable reports = database.Tables[tableName];
                    reports.Rows.Remove(reports.Rows.Find(id));
                    break;

                case DatabaseConstants.Finance.TABLE_NAME:
                    DataTable finances = database.Tables[tableName];
                    finances.Rows.Remove(finances.Rows.Find(id));
                    break;
            }
        }

        private void removeProgrammer(int id)
        {
            DataTable programmers = database.Tables[DatabaseConstants.Programmer.TABLE_NAME];
            programmers.Rows.Remove(programmers.Rows.Find(id));

            DataTable skills = database.Tables[DatabaseConstants.ProgrammersSkills.TABLE_NAME];
            foreach (DataRow row in skills.Select(DatabaseConstants.ProgrammersSkills.PROGRAMMER_ID + "=" + id))
            {
                skills.Rows.Remove(row);
            }

            DataTable performers = database.Tables[DatabaseConstants.Performer.TABLE_NAME];
            foreach (DataRow row in performers.Select(DatabaseConstants.Performer.PROGRAMMER_ID + "=" + id))
            {
                performers.Rows.Remove(row);
            }
        }

        private void removeProject(int id)
        {
            DataTable projects = database.Tables[DatabaseConstants.Project.TABLE_NAME];
            projects.Rows.Remove(projects.Rows.Find(id));

            DataTable performers = database.Tables[DatabaseConstants.Performer.TABLE_NAME];
            List<DataRow> rowsToRemove = new List<DataRow>();
            foreach (DataRow row in performers.Select(DatabaseConstants.Performer.PROJECT_ID + "=" + id))
            {
                performers.Rows.Remove(row);
            }
        }
        #endregion

        #region some help functions
        internal List<int> createIdsListFromSkillsList(List<string> skills)
        {
            DataTable table = database.Tables[DatabaseConstants.Skill.TABLE_NAME];
            List<int> result = new List<int>();
            foreach (string skill in skills)
            {
                DataRow row = table.Rows.Find(skill);
                if (row == null)
                {
                    row = table.NewRow();
                    int id = table.Rows.Count + 1;
                    row[DatabaseConstants.Skill.ID] = id;
                    row[DatabaseConstants.Skill.SKILL] = skill;
                    table.Rows.Add(row);
                    result.Add(id);
                }
                else
                {
                    result.Add((int)row[DatabaseConstants.Skill.ID]);
                }
            }
            return result;
        }

        internal string getSkillsByIds(List<int> skillsIds)
        {
            StringBuilder result = new StringBuilder();
            foreach (DataRow row in database.Tables[DatabaseConstants.Skill.TABLE_NAME].Rows)
            {
                if (skillsIds.Contains((int) row[DatabaseConstants.Skill.ID]))
                {
                    result.Append(row[DatabaseConstants.Skill.SKILL].ToString() + ", ");
                }
            }
            return result.Remove(result.Length - 2, 2).ToString();
        }

        internal string getPerformersByIds(List<int> list)
        {
            StringBuilder result = new StringBuilder();
            foreach (int id in list)
            {
                DataRow dataRow = database.Tables[DatabaseConstants.Programmer.TABLE_NAME].Rows.Find(id);
                result.Append(dataRow[DatabaseConstants.Programmer.NAME].ToString()).Append(", ");
            }
            return result.Remove(result.Length - 2, 2).ToString();
        }
        #endregion

        internal void save()
        {
            database.WriteXml(FILENAME, XmlWriteMode.WriteSchema);
            DatabaseConstants.IdsKeeper.save();
        }

        private void createNewDatabase()
        {
            DataTable reportsTable = new DataTable(DatabaseConstants.Report.TABLE_NAME);
            reportsTable.Columns.Add(new DataColumn(DatabaseConstants.Report.ID, Type.GetType("System.Int32")));
            reportsTable.Columns.Add(new DataColumn(DatabaseConstants.Report.AUTHOR, Type.GetType("System.Int32")));
            reportsTable.Columns.Add(new DataColumn(DatabaseConstants.Report.CONTENT, Type.GetType("System.String")));
            reportsTable.PrimaryKey = new DataColumn[] { reportsTable.Columns[DatabaseConstants.Report.ID] };
            database.Tables.Add(reportsTable);

            DataTable financeTable = new DataTable(DatabaseConstants.Finance.TABLE_NAME);
            financeTable.Columns.Add(new DataColumn(DatabaseConstants.Finance.ID, Type.GetType("System.Int32")));
            financeTable.Columns.Add(new DataColumn(DatabaseConstants.Finance.INCOME, Type.GetType("System.Int64")));
            financeTable.Columns.Add(new DataColumn(DatabaseConstants.Finance.EXPENSE, Type.GetType("System.Int64")));
            financeTable.Columns.Add(new DataColumn(DatabaseConstants.Finance.PROFIT, Type.GetType("System.Int64")));
            financeTable.Columns.Add(new DataColumn(DatabaseConstants.Finance.DESCRIPTION, Type.GetType("System.String")));
            financeTable.PrimaryKey = new DataColumn[] { financeTable.Columns[DatabaseConstants.Finance.ID] };
            database.Tables.Add(financeTable);

            DataTable projectsTable = new DataTable(DatabaseConstants.Project.TABLE_NAME);
            projectsTable.Columns.Add(new DataColumn(DatabaseConstants.Project.ID, Type.GetType("System.Int32")));
            projectsTable.Columns.Add(new DataColumn(DatabaseConstants.Project.CUSTOMER, Type.GetType("System.String")));
            projectsTable.Columns.Add(new DataColumn(DatabaseConstants.Project.COST, Type.GetType("System.Int64")));
            projectsTable.Columns.Add(new DataColumn(DatabaseConstants.Project.DATE, Type.GetType("System.String")));
            projectsTable.Columns.Add(new DataColumn(DatabaseConstants.Project.DESCRIPTION, Type.GetType("System.String")));
            projectsTable.PrimaryKey = new DataColumn[] { projectsTable.Columns[DatabaseConstants.Project.ID] };
            database.Tables.Add(projectsTable);

            DataTable programmerTable = new DataTable(DatabaseConstants.Programmer.TABLE_NAME);
            programmerTable.Columns.Add(new DataColumn(DatabaseConstants.Programmer.ID, Type.GetType("System.Int32")));
            programmerTable.Columns.Add(new DataColumn(DatabaseConstants.Programmer.NAME, Type.GetType("System.String")));
            programmerTable.Columns.Add(new DataColumn(DatabaseConstants.Programmer.SALARY, Type.GetType("System.Int32")));
            programmerTable.Columns.Add(new DataColumn(DatabaseConstants.Programmer.DESCRIPTION, Type.GetType("System.String")));
            programmerTable.PrimaryKey = new DataColumn[] { programmerTable.Columns[DatabaseConstants.Programmer.ID] };
            database.Tables.Add(programmerTable);

            DataTable skillsTable = new DataTable(DatabaseConstants.Skill.TABLE_NAME);
            skillsTable.Columns.Add(new DataColumn(DatabaseConstants.Skill.ID, Type.GetType("System.Int32")));
            skillsTable.Columns.Add(new DataColumn(DatabaseConstants.Skill.SKILL, Type.GetType("System.String")));
            DataColumn[] skillsKey = new DataColumn[] { skillsTable.Columns[DatabaseConstants.Skill.SKILL] };
            skillsTable.PrimaryKey = skillsKey;
            database.Tables.Add(skillsTable);

            DataTable programmersSkillsTable = new DataTable(DatabaseConstants.ProgrammersSkills.TABLE_NAME);
            programmersSkillsTable.Columns.Add(new DataColumn(DatabaseConstants.ProgrammersSkills.SKILL_ID, Type.GetType("System.Int32")));
            programmersSkillsTable.Columns.Add(new DataColumn(DatabaseConstants.ProgrammersSkills.PROGRAMMER_ID, Type.GetType("System.Int32")));
            DataColumn[] programmerSkillsKey = new DataColumn[] 
            {
                programmersSkillsTable.Columns[0], programmersSkillsTable.Columns[1],
            };
            programmersSkillsTable.PrimaryKey = programmerSkillsKey;//*/
            database.Tables.Add(programmersSkillsTable);

            DataTable performersTable = new DataTable(DatabaseConstants.Performer.TABLE_NAME);
            performersTable.Columns.Add(new DataColumn(DatabaseConstants.Performer.PROGRAMMER_ID, Type.GetType("System.Int32")));
            performersTable.Columns.Add(new DataColumn(DatabaseConstants.Performer.PROJECT_ID, Type.GetType("System.Int32")));
            DataColumn[] performersKey = new DataColumn[] 
            { 
                performersTable.Columns[DatabaseConstants.Performer.PROGRAMMER_ID], 
                performersTable.Columns[DatabaseConstants.Performer.PROJECT_ID], 
            };
            performersTable.PrimaryKey = performersKey;
            database.Tables.Add(performersTable);

            DataRelation reportsRelation = new DataRelation(DatabaseConstants.Relations.REPORT_PROGRAMMER,
                new DataColumn[] { programmerTable.Columns[DatabaseConstants.Programmer.ID] },
                new DataColumn[] { reportsTable.Columns[DatabaseConstants.Report.AUTHOR] });
            database.Relations.Add(reportsRelation);

            DataRelation programmerSkillsRelation = new DataRelation(DatabaseConstants.Relations.SKILL_PROGRAMMERSKILL,
                new DataColumn[] { skillsTable.Columns[DatabaseConstants.Skill.ID] },
                new DataColumn[] { programmersSkillsTable.Columns[DatabaseConstants.ProgrammersSkills.SKILL_ID] });
            database.Relations.Add(programmerSkillsRelation);

            /*DataRelation skillsRelation = new DataRelation(DatabaseConstants.Relations.SKILL_PROGRAMMER,
                new DataColumn[] { programmersSkillsTable.Columns[DatabaseConstants.ProgrammersSkills.PROGRAMMER_ID] },
                new DataColumn[] { programmerTable.Columns[DatabaseConstants.Programmer.ID] });
            database.Relations.Add(skillsRelation);//*/

            DataRelation performersRelation = new DataRelation(DatabaseConstants.Relations.PROJECT_PERFORMER,
                new DataColumn[] { projectsTable.Columns[DatabaseConstants.Project.ID] },
                new DataColumn[] { performersTable.Columns[DatabaseConstants.Performer.PROJECT_ID] });
            database.Relations.Add(performersRelation);

            DataRelation programmerPerformerRelation = new DataRelation(DatabaseConstants.Relations.PROGRAMMER_PERFORMER,
                new DataColumn[] { programmerTable.Columns[DatabaseConstants.Programmer.ID] },
                new DataColumn[] { performersTable.Columns[DatabaseConstants.Performer.PROGRAMMER_ID] });
            database.Relations.Add(programmerPerformerRelation);

            database.WriteXml(FILENAME, XmlWriteMode.WriteSchema);
        }

    }
}
