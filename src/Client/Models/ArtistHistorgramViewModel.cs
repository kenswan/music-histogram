﻿namespace BlazorMusic.Client.Models;

public class ArtistHistorgramViewModel
{
    public int[] Releases { get; set; }

    public int[] Years { get; set; }

    public string ArtistName { get; set; }

    public string CsvFormat { get; set; }
}
