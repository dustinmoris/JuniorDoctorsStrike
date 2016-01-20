namespace JuniorDoctorsStrike.Apis

module TwitterClient =
    open System
    open System.Net.Http
    open System.Net
    open Newtonsoft.Json

    type ResultType =
        | Recent
        | Popular
        | Mixed

    type User = {
        Name : string
        ImageUrl : string
    }

    type Tweet = {
        Created : DateTime
        TimeSinceCreated : DateTime
        User : User
        Text : string
    }
    
    let createHttpClient =
        let client = new HttpClient()
        client.DefaultRequestHeaders.Add(
            "Authorization",
            "")
        client

    let createSearchQuery valuesToSearch =
        let searchValue = 
            valuesToSearch
            |> String.concat " "   
            |> WebUtility.UrlEncode     
        sprintf "%s AND -filter:retweets AND -filter:replies" searchValue

    let convertResultType resultType =
        match resultType with
        | Recent    -> "recent"
        | Popular   -> "popular"
        | Mixed     -> "mixed"

    let searchTweetsForHashtags hashtags resultType =
        let client = createHttpClient
        let query = createSearchQuery hashtags
        let url =
            sprintf "search/tweets.json?q=%s&result_type=%s&count=30"
                query
                (convertResultType resultType)
        async {
            let! response =
                client.GetAsync(url)
                |> Async.AwaitTask
            
            response.EnsureSuccessStatusCode() |> ignore

            let! body =
                response.Content.ReadAsStringAsync()
                |> Async.AwaitTask


            return body 
        }

    let parseTweets jsonPayload =
        let obj = JuniorDoctorsStrike.Common.JsonSerializer.Deserialize(jsonPayload)
//        let obj = JsonConvert.DeserializeObject jsonPayload
//        obj.statuses
//        |>
        ""