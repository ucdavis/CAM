﻿using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class RequestTemplate : RequestBase
    {
        [Display(Name="Template Name")]
        public virtual string Name { get; set; }
    }

    public class RequestTemplateMap : ClassMap<RequestTemplate>
    {
        public RequestTemplateMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            References(x => x.Site);
            References(x => x.Unit);
            Map(x => x.NeedsEmail);
            Map(x => x.DefaultSave);
        }
    }

}