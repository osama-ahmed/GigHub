using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class Notification
    {
        public int Id { get; private set; }

        public DateTime DateTime { get; private set; }

        public NotificationType Type { get; private set; }

        public DateTime? OrginalDateTime { get; set; }

        public string OrginalVenue { get; set; }

        [Required]
        public Gig Gig { get; private set; }

        protected Notification()
        {

        }

        private Notification(NotificationType type, Gig gig)
        {
            if (gig == null)
                throw new ArgumentNullException("gig");

            Type = type;
            Gig = gig;
            DateTime = DateTime.Now;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCreated, gig);
        }

        public static Notification GigUpdated(Gig gig, DateTime dateTime, string venue)
        {
            var notification= new Notification(NotificationType.GigUpdated, gig);
            notification.OrginalDateTime = dateTime;
            notification.OrginalVenue = venue;

            return notification;
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }
    }
}