using System;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class CloseRequest : DomainObject
    {
        public CloseRequest()
        {
            DateRequested = DateTime.Now;
            IsPending = true;
        }

        public virtual string LoginId { get; set; }
        public virtual DateTime DateRequested { get; set; }
        public virtual string RequestedBy { get; set; }
        public virtual bool IsPending { get; set; }
    }

    public class CloseRequestMap : ClassMap<CloseRequest>
    {
        public CloseRequestMap()
        {
            Id(x => x.Id);

            Map(x => x.LoginId);
            Map(x => x.DateRequested);
            Map(x => x.RequestedBy);
            Map(x => x.IsPending);
        }
    }
}
