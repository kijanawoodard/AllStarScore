﻿using System;
using System.Collections.Generic;
using System.Linq;
using AllStarScore.Extensions;
using AllStarScore.Models.Commands;

namespace AllStarScore.Models
{
    public class Competition : ICanBeUpdatedByCommand, IBelongToCompany, IGenerateMyId
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
		public DateTimeOffset FirstDay { get; set; }

        public int NumberOfDays { get; set; }
		public DateTimeOffset LastDay { get { return FirstDay.AddDays(NumberOfDays - 1); } }

        public int NumberOfPerformances { get; set; }
        public bool IsWorldsCompetition { get; set; }

		public IEnumerable<DateTime> Days { get { return FirstDay.GetDateRange(LastDay); } }
        public string Display { get { return string.Format("{0} {1: MMM dd, yyyy}", Name, FirstDay); } }

        public string CompanyId { get; set; }
        public string LastCommand { get; set; }
        public string LastCommandBy { get; set; }
        public DateTime LastCommandDate { get; set; }

        public Competition()
        {
            NumberOfDays = 1;
        }

        public void Update(CompetitionCreateCommand command)
        {
            Name = command.CompetitionName;
            FirstDay = command.FirstDay;
            NumberOfDays = command.NumberOfDays;
            NumberOfPerformances = command.NumberOfPerformances;
        	IsWorldsCompetition = command.IsWorldsCompetition;

            this.RegisterCommand(command);
        }

        public static string FormatId(string companyId)
        {
            return companyId + "/competitions/";
        }

        public string GenerateId()
        {
            return FormatId(CompanyId);
        }

        public bool Equals(Competition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Competition)) return false;
            return Equals((Competition) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Id: {1}, FirstDay: {2}", Name, Id, FirstDay);
        }
    }
}