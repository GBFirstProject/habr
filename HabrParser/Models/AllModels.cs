using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace HabrParser.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Article
    {
        public string id { get; set; }
        public DateTime timePublished { get; set; }
        public bool isCorporative { get; set; }
        public string lang { get; set; }
        public string titleHtml { get; set; }
        public LeadData leadData { get; set; }
        public string editorVersion { get; set; }
        public string postType { get; set; }
        public List<object> postLabels { get; set; }
        public Author author { get; set; }
        public Statistics statistics { get; set; }
        public List<Hub> hubs { get; set; }
        public List<Flow> flows { get; set; }
        public object relatedData { get; set; }
        public string textHtml { get; set; }
        public List<Tag> tags { get; set; }
        public Metadata metadata { get; set; }
        public List<object> polls { get; set; }
        public bool commentsEnabled { get; set; }
        public bool rulesRemindEnabled { get; set; }
        public bool votesEnabled { get; set; }
        public string status { get; set; }
        public object plannedPublishTime { get; set; }
        public object @checked { get; set; }
        public bool hasPinnedComments { get; set; }
        public object format { get; set; }
        public bool isEditorial { get; set; }
    }

    public class ActiveBlocks
    {
        public string salary { get; set; }
    }

    public class Adblock
    {
        public bool hasAcceptableAdsFilter { get; set; }
        public bool hasAdblock { get; set; }
    }

    public class Agreement
    {
        public List<object> errors { get; set; }
        public object @ref { get; set; }
        public bool value { get; set; }
    }

    public class AnnouncementCommentingStatuses
    {
    }

    public class AnnouncementComments
    {
    }

    public class AnnouncementCommentThreads
    {
    }

    public class Announcements
    {
    }

    public class ArticleComments
    {
    }

    public class ArticleIds
    {
    }

    public class ArticlePinnedComments
    {
    }

    public class ArticleRefs
    {
    }

    public class Articles
    {
    }

    public class ArticlesIds
    {
    }

    public class ArticlesLangEnglish
    {
        public List<object> errors { get; set; }
        public object @ref { get; set; }
        public bool value { get; set; }
    }

    public class ArticlesLangRussian
    {
        public List<object> errors { get; set; }
        public object @ref { get; set; }
        public bool value { get; set; }
    }

    public class ArticlesList
    {
        public ArticlesList articlesList { get; set; }
        public ArticlesIds articlesIds { get; set; }
        public bool isLoading { get; set; }
        public PagesCount pagesCount { get; set; }
        public Route route { get; set; }
        public object reasonsList { get; set; }
        public string view { get; set; }
        public LastVisitedRoute lastVisitedRoute { get; set; }
        public List<string> ssrCommentsArticleIds { get; set; }
        public Karma karma { get; set; }

        //[JsonProperty("1")]
        public Article article { get; set; }
    }

    public class Author
    {
        public string id { get; set; }
        public string alias { get; set; }
        public string fullname { get; set; }
        public string avatarUrl { get; set; }
        public string speciality { get; set; }
        public ScoreStats scoreStats { get; set; }
        public int rating { get; set; }
        public object relatedData { get; set; }
        public List<Contact> contacts { get; set; }
        public List<AuthorContact> authorContacts { get; set; }
        public PaymentDetails paymentDetails { get; set; }
        public string logo { get; set; }
        public string title { get; set; }
        public string link { get; set; }
    }

    public class AuthorContact
    {
        public string title { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public object siteTitle { get; set; }
        public object favicon { get; set; }
    }

    public class AuthorContribution
    {
        public Authors authors { get; set; }
    }

    public class AuthorFollowed
    {
    }

    public class AuthorFollowers
    {
    }

    public class AuthorIds
    {
    }

    public class AuthorProfiles
    {
    }

    public class AuthorRefs
    {
    }

    public class Authors
    {
    }

    public class AuthorStatistics
    {
        public ArticleRefs articleRefs { get; set; }
        public ArticleIds articleIds { get; set; }
        public PagesCount pagesCount { get; set; }
        public Route route { get; set; }
        public List<object> viewsCount { get; set; }
        public MaxStatsCount maxStatsCount { get; set; }
    }

    public class BetaTest
    {
        public object currentAnnouncement { get; set; }
        public Announcements announcements { get; set; }
        public object announcementCards { get; set; }
        public AnnouncementComments announcementComments { get; set; }
        public AnnouncementCommentThreads announcementCommentThreads { get; set; }
        public AnnouncementCommentingStatuses announcementCommentingStatuses { get; set; }
        public List<object> archivedList { get; set; }
    }

    public class Button
    {
        public string title { get; set; }
        public string link { get; set; }
        public string colorType { get; set; }
    }

    public class Career
    {
        public List<object> seoLandings { get; set; }
        public string hubs { get; set; }
    }

    public class CommentAccess
    {
    }

    public class Comments
    {
        public ArticleComments articleComments { get; set; }
        public ArticlePinnedComments articlePinnedComments { get; set; }
        public object searchCommentsResults { get; set; }
        public object pagesCount { get; set; }
        public CommentAccess commentAccess { get; set; }
        public ScrollParents scrollParents { get; set; }
        public PageArticleComments pageArticleComments { get; set; }
    }

    public class Companies
    {
        public CompanyRefs companyRefs { get; set; }
        public CompanyIds companyIds { get; set; }
        public CompanyTopIds companyTopIds { get; set; }
        public PagesCount pagesCount { get; set; }
        public CompanyProfiles companyProfiles { get; set; }
        public List<object> companiesCategories { get; set; }
        public int companiesCategoriesTotalCount { get; set; }
        public CompaniesWidgets companiesWidgets { get; set; }
        public CompaniesWorkers companiesWorkers { get; set; }
        public CompaniesFans companiesFans { get; set; }
        public Route route { get; set; }
        public bool isLoading { get; set; }
        public bool companyWorkersLoading { get; set; }
        public bool companyFansLoading { get; set; }
        public Vacancies vacancies { get; set; }
        public CompaniesGalleries companiesGalleries { get; set; }
        public CompaniesBanners companiesBanners { get; set; }
        public CompaniesLandingVacancies companiesLandingVacancies { get; set; }
        public CompaniesTechnologies companiesTechnologies { get; set; }
        public object workplaceInfo { get; set; }
    }

    public class CompaniesBanners
    {
    }

    public class CompaniesContribution
    {
        public Hubs hubs { get; set; }
        public Flows flows { get; set; }
        public CompanyRefs companyRefs { get; set; }
    }

    public class Hubs
    {

    }

    public class Flows
    {

    }

    public class CompaniesFans
    {
    }

    public class CompaniesGalleries
    {
    }

    public class CompaniesLandingVacancies
    {
    }

    public class CompaniesTechnologies
    {
    }

    public class CompaniesWidgets
    {
    }

    public class CompaniesWorkers
    {
    }

    public class CompanyAdmin
    {
        public object companyInfo { get; set; }
        public bool companyInfoLoading { get; set; }
        public object faqArticles { get; set; }
        public object brandingPreviewImageUrl { get; set; }
        public int jivoStatus { get; set; }
    }

    public class CompanyHubsContribution
    {
        public ContributionRefs contributionRefs { get; set; }
    }

    public class CompanyIds
    {
    }

    public class CompanyProfiles
    {
    }

    public class CompanyRefs
    {
    }

    public class CompanyTopIds
    {
    }

    public class Contact
    {
        public string title { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public object siteTitle { get; set; }
        public object favicon { get; set; }
    }

    public class ContributionRefs
    {
        public HubRefs hubRefs { get; set; }
        public HubIds hubIds { get; set; }
    }

    public class Conversation
    {
        public List<object> messages { get; set; }
        public object respondent { get; set; }
        public bool isLoadMore { get; set; }
    }

    public class Conversations
    {
        public List<object> conversations { get; set; }
        public int unreadCount { get; set; }
        public int pagesCount { get; set; }
    }

    public class DesktopState
    {
        public object desktopFl { get; set; }
        public object desktopHl { get; set; }
        public bool isChecked { get; set; }
        public bool isLoginDemanded { get; set; }
    }

    public class Digest
    {
        public List<object> errors { get; set; }
        public object @ref { get; set; }
        public bool value { get; set; }
    }

    public class Docs
    {
        public Menu menu { get; set; }
        public Articles articles { get; set; }
        public List<object> mainMenu { get; set; }
        public Loading loading { get; set; }
    }

    public class Email
    {
        public List<object> errors { get; set; }
        public object @ref { get; set; }
        public bool value { get; set; }
    }

    public class Feature
    {
        public bool isProbablyVisible { get; set; }
    }

    public class Flow
    {
        public string id { get; set; }
        public string alias { get; set; }
        public string title { get; set; }
        public string titleHtml { get; set; }
        public Route route { get; set; }
        public Updates updates { get; set; }
        public List<Flow> flows { get; set; }
    }

    public class Global
    {
        public bool isPwa { get; set; }
        public string device { get; set; }
        public bool isHabrCom { get; set; }
    }

    public class Hub
    {
        public string id { get; set; }
        public string alias { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string titleHtml { get; set; }
        public bool isProfiled { get; set; }
        public object relatedData { get; set; }
        public HubRefs hubRefs { get; set; }
        public HubIds hubIds { get; set; }
        public PagesCount pagesCount { get; set; }
        public bool isLoading { get; set; }
        public Route route { get; set; }
    }

    public class HubIds
    {
    }

    public class HubRefs
    {
    }

    public class HubsBlock
    {
        public HubRefs hubRefs { get; set; }
        public HubIds hubIds { get; set; }
    }

    public class I18n
    {
        public string fl { get; set; }
        public string hl { get; set; }
    }

    public class Info
    {
        public InfoPage infoPage { get; set; }
        public bool isLoading { get; set; }
    }

    public class InfoPage
    {
    }

    public class Inputs
    {
        public UiLang uiLang { get; set; }
        public ArticlesLangEnglish articlesLangEnglish { get; set; }
        public ArticlesLangRussian articlesLangRussian { get; set; }
        public Agreement agreement { get; set; }
        public Email email { get; set; }
        public Digest digest { get; set; }
    }

    public class Items
    {
    }

    public class Karma
    {
        public object userReasonsList { get; set; }
    }

    public class KarmaResetInfo
    {
        public object canReincarnate { get; set; }
        public object wasReincarnated { get; set; }
        public object currentScore { get; set; }
    }

    public class LastVisitedRoute
    {
    }

    public class LeadData
    {
        public string textHtml { get; set; }
        public object imageUrl { get; set; }
        public object buttonTextHtml { get; set; }
        public object image { get; set; }
    }

    public class Loading
    {
        public bool main { get; set; }
        public bool dropdown { get; set; }
        public bool article { get; set; }
    }

    public class Location
    {
        public UrlStruct urlStruct { get; set; }
    }

    public class MarkedRead
    {
    }

    public class MarkedViewedSilently
    {
    }

    public class MaxStatsCount
    {
    }

    public class Me
    {
        public object user { get; set; }
        public bool ppgDemanded { get; set; }
        public KarmaResetInfo karmaResetInfo { get; set; }
        public object notes { get; set; }
    }

    public class Menu
    {
    }

    public class Metadata
    {
        public List<object> stylesUrls { get; set; }
        public List<object> scriptUrls { get; set; }
        public string shareImageUrl { get; set; }
        public int shareImageWidth { get; set; }
        public int shareImageHeight { get; set; }
        public string vkShareImageUrl { get; set; }
        public string schemaJsonLd { get; set; }
        public string metaDescription { get; set; }
        public object mainImageUrl { get; set; }
        public bool amp { get; set; }
        public List<object> customTrackerLinks { get; set; }
    }

    public class MostReadingList
    {
        public List<object> mostReadingListIds { get; set; }
        public object mostReadingListRefs { get; set; }
        public object promoPost { get; set; }
    }

    public class PageArticleComments
    {
        public int lastViewedComment { get; set; }
        public object postId { get; set; }
        public string lastCommentTimestamp { get; set; }
        public List<object> moderated { get; set; }
        public List<object> moderatedIds { get; set; }
        public string commentRoute { get; set; }
    }

    public class PagesCache
    {
    }

    public class PagesCount
    {
    }

    public class Params
    {
        public string flowName { get; set; }
    }

    public class PaymentDetails
    {
        public object paymentYandexMoney { get; set; }
        public object paymentPayPalMe { get; set; }
        public object paymentWebmoney { get; set; }
    }

    public class PinnedPost
    {
        public object pinnedPost { get; set; }
    }

    public class Ppa
    {
        public Articles articles { get; set; }
        public object card { get; set; }
        public object transactions { get; set; }
        public object totalTransactions { get; set; }
        public object isAccessible { get; set; }
    }

    public class PrevScrollY
    {
    }

    public class ProjectsBlocks
    {
        public ActiveBlocks activeBlocks { get; set; }
    }

    public class PromoData
    {
        public bool isLoading { get; set; }
        public bool hasLoaded { get; set; }
        public object featurer { get; set; }
        public object megaposts { get; set; }
        public object promoLinks { get; set; }
        public object promoPosts { get; set; }
    }

    public class PullRefresh
    {
        public bool shouldRefresh { get; set; }
    }

    public class Query
    {
    }

    public class Root
    {
        public Adblock adblock { get; set; }
        public ArticlesList articlesList { get; set; }
        public AuthorContribution authorContribution { get; set; }
        public BetaTest betaTest { get; set; }
        public AuthorStatistics authorStatistics { get; set; }
        public Career career { get; set; }
        public Comments comments { get; set; }
        public Companies companies { get; set; }
        public CompanyAdmin companyAdmin { get; set; }
        public CompaniesContribution companiesContribution { get; set; }
        public CompanyHubsContribution companyHubsContribution { get; set; }
        public Conversation conversation { get; set; }
        public Conversations conversations { get; set; }
        public DesktopState desktopState { get; set; }
        public Docs docs { get; set; }
        public Feature feature { get; set; }
        public Flows flows { get; set; }
        public Global global { get; set; }
        public Hubs hubs { get; set; }
        public HubsBlock hubsBlock { get; set; }
        public I18n i18n { get; set; }
        public Info info { get; set; }
        public Location location { get; set; }
        public Me me { get; set; }
        public MostReadingList mostReadingList { get; set; }
        public PinnedPost pinnedPost { get; set; }
        public Ppa ppa { get; set; }
        public ProjectsBlocks projectsBlocks { get; set; }
        public PromoData promoData { get; set; }
        public PullRefresh pullRefresh { get; set; }
        public Sandbox sandbox { get; set; }
        public Search search { get; set; }
        public SettingsOther settingsOther { get; set; }
        public SimilarList similarList { get; set; }
        public Ssr ssr { get; set; }
        public Stories stories { get; set; }
        public Technotext technotext { get; set; }
        public UserHubsContribution userHubsContribution { get; set; }
        public UserInvites userInvites { get; set; }
        public UserVotes userVotes { get; set; }
        public Users users { get; set; }
        public Viewport viewport { get; set; }
        public Tracker tracker { get; set; }
    }

    public class Route
    {
        public string name { get; set; }
        public Params @params { get; set; }
    }

    public class Sandbox
    {
        public List<object> articleIds { get; set; }
        public ArticleRefs articleRefs { get; set; }
        public object pagesCount { get; set; }
        public Route route { get; set; }
        public LastVisitedRoute lastVisitedRoute { get; set; }
        public bool isLoading { get; set; }
    }

    public class ScoreStats
    {
        public int score { get; set; }
        public int votesCount { get; set; }
    }

    public class ScrollParents
    {
    }

    public class Search
    {
        public object searchQueryError { get; set; }
    }

    public class SettingsOther
    {
        public Inputs inputs { get; set; }
    }

    public class SimilarList
    {
        public List<object> similarListIds { get; set; }
        public object similarListRefs { get; set; }
    }

    public class Slide
    {
        public string id { get; set; }
        public string image { get; set; }
        public Button button { get; set; }
    }

    public class Ssr
    {
        public object error { get; set; }
        public bool isDataLoaded { get; set; }
        public bool isDataLoading { get; set; }
        public bool isHydrationFailed { get; set; }
        public bool isServer { get; set; }
    }

    public class Statistics
    {
        public int commentsCount { get; set; }
        public int favoritesCount { get; set; }
        public int readingCount { get; set; }
        public int score { get; set; }
        public int votesCount { get; set; }
        public int votesCountPlus { get; set; }
        public int votesCountMinus { get; set; }
    }

    public class Stories
    {
        public List<Story> stories { get; set; }
        public string id { get; set; }
        public Author author { get; set; }
        public string title { get; set; }
        public string lang { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public List<Slide> slides { get; set; }
    }

    public class Story
    {
        public string id { get; set; }
        public Author author { get; set; }
        public string title { get; set; }
        public string lang { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public List<Slide> slides { get; set; }
    }

    public class Tag
    {
        public string titleHtml { get; set; }
    }

    public class Technotext
    {
        public List<object> years { get; set; }
        public object technotextDocForNominees { get; set; }
        public object technotextDocForWinners { get; set; }
        public TechnotextInfo technotextInfo { get; set; }
        public bool technotextInfoLoading { get; set; }
        public TechnotextWinners technotextWinners { get; set; }
        public bool technotextWinnersLoading { get; set; }
    }

    public class TechnotextInfo
    {
    }

    public class TechnotextWinners
    {
    }

    public class Tracker
    {
        public Items items { get; set; }
        public PagesCache pagesCache { get; set; }
        public MarkedViewedSilently markedViewedSilently { get; set; }
        public MarkedRead markedRead { get; set; }
        public UnreadCounters unreadCounters { get; set; }
        public UnviewedCounters unviewedCounters { get; set; }
    }

    public class UiLang
    {
        public List<object> errors { get; set; }
        public object @ref { get; set; }
        public string value { get; set; }
    }

    public class UnreadCounters
    {
        public object applications { get; set; }
        public object system { get; set; }
        public object mentions { get; set; }
        public object subscribers { get; set; }
        public object posts_and_comments { get; set; }
    }

    public class UnusedInvitesRefs
    {
    }

    public class UnviewedCounters
    {
        public object applications { get; set; }
        public object system { get; set; }
        public object mentions { get; set; }
        public object subscribers { get; set; }
        public object posts_and_comments { get; set; }
    }

    public class Updates
    {
        public object countNewPostsBySubscription { get; set; }
        public int countNewPostsAll { get; set; }
        public int countNewNewsAll { get; set; }
    }

    public class UrlStruct
    {
        public object protocol { get; set; }
        public object slashes { get; set; }
        public object auth { get; set; }
        public object host { get; set; }
        public object port { get; set; }
        public object hostname { get; set; }
        public object hash { get; set; }
        public object search { get; set; }
        public Query query { get; set; }
        public object pathname { get; set; }
        public object path { get; set; }
        public string href { get; set; }
    }

    public class UsedInvitesRefs
    {
    }

    public class UserHubs
    {
    }

    public class UserHubsContribution
    {
        public ContributionRefs contributionRefs { get; set; }
    }

    public class UserInvitations
    {
    }

    public class UserInvites
    {
        public int availableInvites { get; set; }
        public List<object> usedInvitesIds { get; set; }
        public UsedInvitesRefs usedInvitesRefs { get; set; }
        public int usedInvitesPagesCount { get; set; }
        public List<object> unusedInvitesIds { get; set; }
        public UnusedInvitesRefs unusedInvitesRefs { get; set; }
        public int unusedInvitesPagesCount { get; set; }
    }

    public class Users
    {
        public AuthorRefs authorRefs { get; set; }
        public AuthorIds authorIds { get; set; }
        public PagesCount pagesCount { get; set; }
        public AuthorProfiles authorProfiles { get; set; }
        public UserHubs userHubs { get; set; }
        public UserInvitations userInvitations { get; set; }
        public AuthorFollowers authorFollowers { get; set; }
        public AuthorFollowed authorFollowed { get; set; }
        public UserSpecialization userSpecialization { get; set; }
        public List<object> karmaStats { get; set; }
        public object statistics { get; set; }
        public bool isLoading { get; set; }
        public bool authorFollowersLoading { get; set; }
        public bool authorFollowedLoading { get; set; }
        public bool userHubsLoading { get; set; }
        public bool userInvitationsLoading { get; set; }
        public Route route { get; set; }
    }

    public class UserSpecialization
    {
    }

    public class UserVotes
    {
        public List<object> karmaVotesList { get; set; }
        public object karmaVotesPagesCount { get; set; }
        public bool karmaVotesListLoading { get; set; }
        public List<object> commentsVotesList { get; set; }
        public object commentsVotesPagesCount { get; set; }
        public bool commentsVotesListLoading { get; set; }
        public List<object> postsVotesList { get; set; }
        public object postsVotesPagesCount { get; set; }
        public bool postsVotesListLoading { get; set; }
        public List<object> userVotesList { get; set; }
        public object userVotesPagesCount { get; set; }
        public bool userVotesListLoading { get; set; }
    }

    public class Vacancies
    {
    }

    public class Viewport
    {
        public PrevScrollY prevScrollY { get; set; }
        public int scrollY { get; set; }
        public int width { get; set; }
    }






}
