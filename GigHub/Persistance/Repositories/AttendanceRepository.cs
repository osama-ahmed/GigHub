﻿using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Attendance> GetUserAttendances(string userId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .ToList();
        }

        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList();
        }

        public Attendance GetAttendance(int gigId, string userId)
        {
            return _context.Attendances
                .SingleOrDefault(a => a.GigId == gigId && a.AttendeeId == userId);
        }

        public void Add(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
        }
    }
}