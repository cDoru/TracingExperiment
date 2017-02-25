using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TracingExperiment.Tracing.Database.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}