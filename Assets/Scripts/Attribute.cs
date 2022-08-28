using System.Collections;
using System.Collections.Generic;
using System;
public class Attribute
{
    public DateTime createdAt{set;get;}
    public DateTime updatedAt{set;get;}
    public DateTime publishedAt{set;get;}

    public string url1 { get; set; }
    public string text { get; set; }
    public Media picture { get; set; }
    public Media video { get; set; }
}