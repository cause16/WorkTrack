﻿namespace WorkTrack.Models
{
    public class Position
    {
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public string PositionName { get; set; } = null!;
        public List<Employee> Employees { get; set; } = new List<Employee>();
	}
}
