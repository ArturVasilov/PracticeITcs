﻿using System;
using System.Collections;
using System.Collections.Generic;

using System.Text.RegularExpressions;

namespace DocumentsSecurity
{
    class Project : Document
    {
        private string customer;

        private long cost;

        private string date;

        private LinkedList<String> performersNames;

        public Project(long id, string description, string customer, long cost, string date, params string[] performersNames) 
            : base(id, DocumentType.Project, description)
        {
            Customer = customer;
            Date = date;
            Cost = cost;
            this.performersNames = new LinkedList<String>();
            foreach (string performer in performersNames)
            {
                this.performersNames.AddLast(performer);
            }
        }

        public string Customer
        {
            get { return customer; }
            set { customer = value; }
        }

        public long Cost
        {
            get { return cost; }
            set
            {
                if (value > 0)
                {
                    cost = value;
                }
                else
                {
                    throw new ArgumentException("project cost is less than zero!");
                }
            }
        }

        public string Date
        {
            get { return date; }
            set
            {
                Regex dateRegex = new Regex("[0-9]{2}/[0-9]{2}/[0-9]{4}");
                if (dateRegex.IsMatch(value))
                {
                    date = value;
                }
                else
                {
                    throw new ArgumentException("Date has irregular format. Use DD/MM/YYYY instead.")
                }
            }
        }

        public void addPerformer(string name)
        {
            performersNames.AddLast(name);
        }

        public void removePerformer(string name)
        {
            performersNames.Remove(name);
        }
    }
}
