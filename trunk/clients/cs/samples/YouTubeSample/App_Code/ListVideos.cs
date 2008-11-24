using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.YouTube; 
using System.Net;



/// <summary>
/// Summary description for VideoList
/// </summary>
public static class ListVideos
{
    public static IEnumerable<Video> MostPopular()
    {
        return GetVideos(YouTubeQuery.MostViewedVideo);
    }

    public static IEnumerable<Video> YourVideos()
    {
        return GetVideos(YouTubeQuery.DefaultUploads);
    }

    public static IEnumerable<Video> MostCommented()
    {
        return GetVideos(YouTubeQuery.MostDiscussedVideo);
    }

    public static void Update(Video v)
    {
        v.Update();
    }



    public static IEnumerable<Playlist> PlayLists()
    {
        YouTubeRequestSettings settings = new YouTubeRequestSettings("YouTubeAspSample",
                                            HttpContext.Current.Session["token"] as string,
                                            "ytapi-FrankMantek-TestaccountforGD-sjgv537n-0",
                                            "AI39si4v3E6oIYiI60ndCNDqnPP5lCqO28DSvvDPnQt-Mqia5uPz2e4E-gMSBVwHXwyn_LF1tWox4LyM-0YQd2o4i_3GcXxa2Q"
                                            );
        settings.AutoPaging = true;
        YouTubeRequest request = new YouTubeRequest(settings);
        Feed<Playlist> feed = null;


        try
        {
            feed = request.GetPlaylistsFeed(null);
        }
        catch (GDataRequestException gdre)
        {
            HttpWebResponse response = (HttpWebResponse)gdre.Response;
        }
        return feed != null ? feed.Entries : null;
    }



    public static IEnumerable<Video> Search(string videoQuery, string author, string orderby, bool racy, string time, string category)
    {
        YouTubeQuery query = new YouTubeQuery(YouTubeQuery.TopRatedVideo);
        if (String.IsNullOrEmpty(videoQuery) != true)
        {
            query.Query = videoQuery;
        }
        if (String.IsNullOrEmpty(author) != true)
        {
            query.Author = author;
        }
        if (String.IsNullOrEmpty(orderby) != true)
        {
            query.OrderBy = orderby;
        }
        if (racy == true)
        {
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;
        }
        if (String.IsNullOrEmpty(time) != true)
        {
            if (time == "All Time")
                query.Time = YouTubeQuery.UploadTime.AllTime;
            else if (time == "Today")
                query.Time = YouTubeQuery.UploadTime.Today;
            else if (time == "This Week")
                query.Time = YouTubeQuery.UploadTime.ThisWeek;
            else if (time == "This Month")
                query.Time = YouTubeQuery.UploadTime.ThisMonth;
        }
        if (String.IsNullOrEmpty(category) != true)
        {
            QueryCategory q  = new QueryCategory(new AtomCategory(category));
            query.Categories.Add(q);
        }
        return ListVideos.GetVideos(query);
    }



    private static IEnumerable<Video> GetVideos(string videofeed)
    {
        YouTubeQuery query = new YouTubeQuery(videofeed);
        return ListVideos.GetVideos(query);
    }

    private static IEnumerable<Video> GetVideos(YouTubeQuery q)
    {
        YouTubeRequestSettings settings = new YouTubeRequestSettings("YouTubeAspSample", 
                                            HttpContext.Current.Session["token"] as string,
                                            "ytapi-FrankMantek-TestaccountforGD-sjgv537n-0",
                                            "AI39si4v3E6oIYiI60ndCNDqnPP5lCqO28DSvvDPnQt-Mqia5uPz2e4E-gMSBVwHXwyn_LF1tWox4LyM-0YQd2o4i_3GcXxa2Q"
                                            );
        settings.AutoPaging = true;
        YouTubeRequest request = new YouTubeRequest(settings);
        Feed<Video> feed = null; 


        try
        {
            feed = request.GetFeed<Video>(q);
        }
        catch (GDataRequestException gdre)
        {
            HttpWebResponse response = (HttpWebResponse)gdre.Response;
        }
        return feed != null ? feed.Entries : null;
    }

}
