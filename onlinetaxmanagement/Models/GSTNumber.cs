//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace onlinetaxmanagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GSTNumber
    {
        public int GnId { get; set; }
        public int Uid { get; set; }
        public string GSTNumber1 { get; set; }
    
        public virtual Registration Registration { get; set; }
    }
}
