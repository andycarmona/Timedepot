﻿using System.Collections.Generic;

namespace PdfReportSamples.Models
{
    public class Person
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string LastName { get; set; }
        public CustomerType Type { set; get; }
        public IList<Phone> Phones { get; set; }
    }
}
