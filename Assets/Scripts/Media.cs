using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Media
{
    public Data data { set; get; }
}

public class Data
{
    public int id { set; get; }
    public MediaAttributes attributes { set; get; }
}

public class MediaAttributes
{
    public string name { set; get; }
    public string alternativeText { set; get; }
    public string caption { set; get; }
    public int width { set; get; }
    public int height { set; get; }
    public Formats formats { set; get; }
    public string hash { set; get; }
    public string ext { set; get; }
    public string mime { set; get; }
    public float size { set; get; }
    public string url { set; get; }
    public string previewUrl { set; get; }
    public string provider { set; get; }
    public string provider_metadata { set; get; }
    public DateTime createdAt { set; get; }
    public DateTime updatedAt { set; get; }
}

public class Formats
{
    public Thumbnail thumbnail { set; get; }
}

public class Thumbnail
{
    public string name { set; get; }
    public string hash { set; get; }
    public string ext { set; get; }
    public string mime { set; get; }
    public string path { set; get; }
    public int width { set; get; }
    public int height { set; get; }
    public float size { set; get; }
    public string url { set; get; }
}