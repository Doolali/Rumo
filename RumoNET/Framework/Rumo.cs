using Microsoft.Extensions.Options;
using RestSharp;
using RumoNET.Enums;
using RumoNET.Models;
using RumoNET.Options;
using System.Collections.Generic;

namespace RumoNET.Framework
{
    public class Rumo
    {
        private const string API_KEY_HEADER = "x-api-key";

        private string _apiKey;
        private string _source;

        public string BaseUrl { get; private set; }

        private IRestClient _restClient;

        public Rumo(IOptions<RumoOptions> options)
        {
            Init(options.Value.Url, options.Value.ApiKey, options.Value.Source);
        }

        public Rumo(string url, string apiKey, string source)
        {
            Init(url, apiKey, source);
        }

        private void Init(string url, string apiKey, string source)
        {
            _apiKey = apiKey;
            _source = source;

            BaseUrl = $"{url}{_source}/";

            _restClient = new RestClient(BaseUrl).UseSerializer(new JsonNetSerializer());
            _restClient.AddDefaultHeader(API_KEY_HEADER, _apiKey);
        }

        /// <summary>
        ///  Submitting/Updating Content
        ///  Object representing your content submission to the database.
        ///  You need both your source and your API key to identify and submit your content. The first time you submit it, the source is created in real-time.
        ///  You should keep each source submission limited to 2000 entries.
        ///  If you add two contents with the same ID to your source, the most recent item takes precedence, overwriting the previous entry
        ///  https://beta.api.rumo.co/{{$source}} 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public RumoSubmission SubmitContent(RumoContent content)
        {
            return SubmitContent(new List<RumoContent> { content });
        }

        /// <summary>
        ///  Submitting/Updating Content
        ///  Object representing your content submission to the database.
        ///  You need both your source and your API key to identify and submit your content. The first time you submit it, the source is created in real-time.
        ///  You should keep each source submission limited to 2000 entries.
        ///  If you add two contents with the same ID to your source, the most recent item takes precedence, overwriting the previous entry
        ///  https://beta.api.rumo.co/{{$source}} 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public RumoSubmission SubmitContent(List<RumoContent> content)
        {
            RestRequest request = new RestRequest(Method.POST);

            request.AddJsonBody(content);

            var r = _restClient.Execute<RumoSubmission>(request);

            return r.Data;
        }

        /// <summary>
        ///  Retrieving Content
        ///  Object representing the verification of your content uploaded to the database.
        ///  https://beta.api.rumo.co/{{$source}} 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public RumoSummary GetContent()
        {
            RestRequest request = new RestRequest(Method.GET);

            return _restClient.Execute<RumoSummary>(request).Data;
        }

        /// <summary>
        ///  Retrieving Content Piece
        ///  Object representing the verification of a specific content piece.
        ///  https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public RumoContent GetContent(string contentId)
        {
            return GetContent(contentId, null);
        }

        /// <summary>
        ///  Retrieving Content Piece
        ///  Object representing the verification of a specific content piece.
        ///  https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}
        /// </summary>
        /// <param name="content"></param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="at">String used to filter by uploaded date: YYYY-MM-DD</param>
        /// <returns></returns>
        public RumoContent GetContent(string contentId, List<string> catalogs = null)
        {
            return GetContent(contentId: contentId, catalogs: catalogs, at: null);
        }

        /// <summary>
        ///  Retrieving Content Piece
        ///  Object representing the verification of a specific content piece.
        ///  https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}
        /// </summary>
        /// <param name="content"></param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="at">String used to filter by uploaded date: YYYY-MM-DD</param>
        /// <returns></returns>
        public RumoContent GetContent(string contentId, List<string> catalogs = null, string at = null)
        {
            RestRequest request = new RestRequest($"/content/{contentId}", Method.GET);

            if (catalogs != null)
            {
                request.AddUrlSegment("catalogs", catalogs);
            }

            if (!string.IsNullOrWhiteSpace(at))
            {
                request.AddUrlSegment("at", at);
            }

            return _restClient.Execute<RumoContent>(request).Data;
        }

        /// <summary>
        /// Retrieving Similar Content
        /// Object representing the verification of similar content to the one you uploaded to the database. You will need at least two content pieces to query similar content.
        /// https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}/similar
        /// <param name="contentId">The Content Id to get similar content for</param>
        /// </summary>
        /// <returns></returns>
        public RumoRecommendation GetSimilarContent(string contentId)
        {
            return GetSimilarContent(contentId, 0);
        }

        /// <summary>
        /// Retrieving Similar Content
        /// Object representing the verification of similar content to the one you uploaded to the database. You will need at least two content pieces to query similar content.
        /// https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}/similar
        /// <param name="contentId">The Content Id to get similar content for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="at">String used to filter by uploaded date: YYYY-MM-DD</param>
        /// <param name="take">The number of recommendations to return</param>
        /// </summary>
        /// <returns></returns>
        public RumoRecommendation GetSimilarContent(string contentId, int take = 0)
        {
            return GetSimilarContent(contentId: contentId, catalogs: null, take: take);
        }

        /// <summary>
        /// Retrieving Similar Content
        /// Object representing the verification of similar content to the one you uploaded to the database. You will need at least two content pieces to query similar content.
        /// https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}/similar
        /// <param name="contentId">The Content Id to get similar content for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="at">String used to filter by uploaded date: YYYY-MM-DD</param>
        /// <param name="take">The number of recommendations to return</param>
        /// </summary>
        /// <returns></returns>
        public RumoRecommendation GetSimilarContent(string contentId, List<string> catalogs = null)
        {
            return GetSimilarContent(contentId: contentId, catalogs: catalogs, take: 0);
        }

        /// <summary>
        /// Retrieving Similar Content
        /// Object representing the verification of similar content to the one you uploaded to the database. You will need at least two content pieces to query similar content.
        /// https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}/similar
        /// <param name="contentId">The Content Id to get similar content for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="at">String used to filter by uploaded date: YYYY-MM-DD</param>
        /// <param name="take">The number of recommendations to return</param>
        /// </summary>
        /// <returns></returns>
        public RumoRecommendation GetSimilarContent(string contentId, List<string> catalogs = null, int take = 0)
        {
            return GetSimilarContent(contentId: contentId, catalogs: catalogs, take: take, at: null);
        }

        /// <summary>
        /// Retrieving Similar Content
        /// Object representing the verification of similar content to the one you uploaded to the database. You will need at least two content pieces to query similar content.
        /// https://beta.api.rumo.co/{{$source}}/content/{{$contentID}}/similar
        /// <param name="contentId">The Content Id to get similar content for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="at">String used to filter by uploaded date: YYYY-MM-DD</param>
        /// <param name="take">The number of recommendations to return</param>
        /// </summary>
        /// <returns></returns>
        public RumoRecommendation GetSimilarContent(string contentId, List<string> catalogs = null, string at = null, int take = 0)
        {
            RestRequest request = new RestRequest($"/content/{contentId}/similar", Method.GET);

            if (catalogs != null)
            {
                request.AddUrlSegment("catalogs", catalogs);
            }

            if (!string.IsNullOrWhiteSpace(at))
            {
                request.AddUrlSegment("at", at);
            }

            if (take > 0)
            {
                request.AddUrlSegment("take", take);
            }

            return _restClient.Execute<RumoRecommendation>(request).Data;
        }

        /// <summary>
        /// Submitting User Events
        /// Object representing user interactions with the content.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/{{$interactionType}}/{{$contentID}} 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="interactionType"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public RumoUserEvent SubmitUserEvent(string userId, RumoInteractionType interactionType, string contentId)
        {
            RestRequest request = new RestRequest($"/users/{userId}/{interactionType.ToString()}/{contentId}", Method.POST);

            return _restClient.Execute<RumoUserEvent>(request).Data;
        }

        /// <summary>
        /// Object listing all the interactions a user does.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/interactions
        /// </summary>
        /// <param name="userId">The User Id to get recommendations for</param>
        /// <returns></returns>
        public List<RumoInteraction> GetInteractions(string userId)
        {
            return GetInteractions(userId, null);
        }

        /// <summary>
        /// Object listing all the interactions a user does.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/interactions
        /// </summary>
        /// <param name="userId">The User Id to get recommendations for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <returns></returns>
        public List<RumoInteraction> GetInteractions(string userId, List<string> catalogs = null)
        {
            RestRequest request = new RestRequest($"/users/{userId}/interactions", Method.GET);

            if (catalogs != null)
            {
                request.AddUrlSegment("catalogs", catalogs);
            }

            return _restClient.Execute<List<RumoInteraction>>(request).Data;
        }

        /// <summary>
        /// Retrieving Personalized Recommendation
        /// Object listing content recommendations using the user’s taste and feedback profile.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/recommendation
        /// </summary>
        /// <param name="userId">The User Id to get recommendations for</param>
        /// <returns></returns>
        public RumoRecommendation GetPersonalizedRecommendation(string userId)
        {
            return GetPersonalizedRecommendation(userId, 0);
        }

        /// <summary>
        /// Retrieving Personalized Recommendation
        /// Object listing content recommendations using the user’s taste and feedback profile.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/recommendation
        /// </summary>
        /// <param name="userId">The User Id to get recommendations for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="take">The number of recommendations to return</param>
        /// <param name="algo">String showing the recommendation algorithm to use. Could be Jaccard Index (with non-weighted categories) or Cosine Similarity
        /// (with weighted categories).</param>
        /// <returns></returns>
        public RumoRecommendation GetPersonalizedRecommendation(string userId, List<string> catalogs = null)
        {
            return GetPersonalizedRecommendation(userId: userId, catalogs: catalogs, take: 0);
        }

        /// <summary>
        /// Retrieving Personalized Recommendation
        /// Object listing content recommendations using the user’s taste and feedback profile.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/recommendation
        /// </summary>
        /// <param name="userId">The User Id to get recommendations for</param>
        /// <param name="take">The number of recommendations to return</param>
        /// <returns></returns>
        public RumoRecommendation GetPersonalizedRecommendation(string userId, int take = 0)
        {
            return GetPersonalizedRecommendation(userId: userId, take: take, catalogs: null);
        }

        /// <summary>
        /// Retrieving Personalized Recommendation
        /// Object listing content recommendations using the user’s taste and feedback profile.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/recommendation
        /// </summary>
        /// <param name="userId">The User Id to get recommendations for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="take">The number of recommendations to return</param>
        /// <returns></returns>
        public RumoRecommendation GetPersonalizedRecommendation(string userId, List<string> catalogs = null, int take = 0)
        {
            return GetPersonalizedRecommendation(userId: userId, catalogs: catalogs, take: take, algo: null);
        }

        /// <summary>
        /// Retrieving Personalized Recommendation
        /// Object listing content recommendations using the user’s taste and feedback profile.
        /// https://beta.api.rumo.co/{{$source}}/users/{{$userID}}/recommendation
        /// </summary>
        /// <param name="userId">The User Id to get recommendations for</param>
        /// <param name="catalogs">List of Strings used to filter by catalog</param>
        /// <param name="take">The number of recommendations to return</param>
        /// <param name="algo">String showing the recommendation algorithm to use. Could be Jaccard Index (with non-weighted categories) or Cosine Similarity
        /// (with weighted categories).</param>
        /// <returns></returns>
        public RumoRecommendation GetPersonalizedRecommendation(string userId, List<string> catalogs = null, int take = 0, string algo = null)
        {
            RestRequest request = new RestRequest($"/users/{userId}/recommendation", Method.GET);

            if (catalogs != null)
            {
                request.AddUrlSegment("catalogs", catalogs);
            }

            if (take > 0)
            {
                request.AddUrlSegment("take", take);
            }

            if (!string.IsNullOrWhiteSpace(algo))
            {
                request.AddUrlSegment("algo", algo);
            }

            return _restClient.Execute<RumoRecommendation>(request).Data;
        }
    }
}