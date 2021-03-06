﻿using System;
using System.Reactive.Threading.Tasks;
using Octokit.Reactive.Internal;
using System.Reactive;

namespace Octokit.Reactive
{
    public class ObservableIssuesClient : IObservableIssuesClient
    {
        readonly IIssuesClient _client;
        readonly IConnection _connection;

        /// <summary>
        /// Client for managing assignees.
        /// </summary>
        public IObservableAssigneesClient Assignee { get; private set; }
        /// <summary>
        /// Client for managing comments.
        /// </summary>
        public IObservableIssueCommentsClient Comment { get; private set; }
        /// <summary>
        /// Client for reading various event information associated with issues/pull requests.  
        /// This is useful both for display on issue/pull request information pages and also to 
        /// determine who should be notified of comments.
        /// </summary>
        public IObservableIssuesEventsClient Events { get; private set; }
        /// <summary>
        /// Client for managing labels.
        /// </summary>
        public IObservableIssuesLabelsClient Labels { get; private set; }
        /// <summary>
        /// Client for managing milestones.
        /// </summary>
        public IObservableMilestonesClient Milestone { get; private set; }

        public ObservableIssuesClient(IGitHubClient client)
        {
            Ensure.ArgumentNotNull(client, "client");

            _client = client.Issue;
            _connection = client.Connection;
            Assignee = new ObservableAssigneesClient(client);
            Events = new ObservableIssuesEventsClient(client);
            Labels = new ObservableIssuesLabelsClient(client);
            Milestone = new ObservableMilestonesClient(client);
            Comment = new ObservableIssueCommentsClient(client);
        }

        /// <summary>
        /// Gets a single Issue by number.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#get-a-single-issue
        /// </remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="number">The issue number</param>
        /// <returns>A signal containing the requested <see cref="Issue"/>s.</returns>
        public IObservable<Issue> Get(string owner, string name, int number)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
            Ensure.ArgumentNotNullOrEmptyString(name, "name");

            return _client.Get(owner, name, number).ToObservable();
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user across all the authenticated user’s visible
        /// repositories including owned repositories, member repositories, and organization repositories.
        /// </summary>
        /// <remarks>
        /// Issues are sorted by the create date descending.
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForCurrent()
        {
            return GetAllForCurrent(ApiOptions.None);
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user across all the authenticated user’s visible
        /// repositories including owned repositories, member repositories, and organization repositories.
        /// </summary>
        /// <param name="options">Options for changing the API response</param>
        /// <remarks>
        /// Issues are sorted by the create date descending.
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForCurrent(ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            return GetAllForCurrent(new IssueRequest(), options);
        }

        /// <summary>
        /// Gets all issues across all the authenticated user’s visible repositories including owned repositories, 
        /// member repositories, and organization repositories.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForCurrent(IssueRequest request)
        {
            Ensure.ArgumentNotNull(request, "request");

            return GetAllForCurrent(request, ApiOptions.None);
        }

        /// <summary>
        /// Gets all issues across all the authenticated user’s visible repositories including owned repositories, 
        /// member repositories, and organization repositories.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForCurrent(IssueRequest request, ApiOptions options)
        {
            Ensure.ArgumentNotNull(request, "request");
            Ensure.ArgumentNotNull(options, "options");

            return _connection.GetAndFlattenAllPages<Issue>(ApiUrls.Issues(), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user across owned and member repositories for the
        /// authenticated user.
        /// </summary>
        /// <remarks>
        /// Issues are sorted by the create date descending.
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOwnedAndMemberRepositories()
        {
            return GetAllForOwnedAndMemberRepositories(new IssueRequest());
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user across owned and member repositories for the
        /// authenticated user.
        /// </summary>
        /// <remarks>
        /// Issues are sorted by the create date descending.
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOwnedAndMemberRepositories(ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, "options");

            return GetAllForOwnedAndMemberRepositories(new IssueRequest(), options);
        }

        /// <summary>
        /// Gets all issues across owned and member repositories for the authenticated user.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOwnedAndMemberRepositories(IssueRequest request)
        {
            Ensure.ArgumentNotNull(request, "request");

            return GetAllForOwnedAndMemberRepositories(request, ApiOptions.None);
        }

        /// <summary>
        /// Gets all issues across owned and member repositories for the authenticated user.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOwnedAndMemberRepositories(IssueRequest request, ApiOptions options)
        {
            Ensure.ArgumentNotNull(request, "request");
            Ensure.ArgumentNotNull(options, "options");

            return _connection.GetAndFlattenAllPages<Issue>(ApiUrls.IssuesForOwnedAndMember(), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user for a given organization for the authenticated user.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="organization">The name of the organization</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOrganization(string organization)
        {
            Ensure.ArgumentNotNullOrEmptyString(organization, "organization");

            return GetAllForOrganization(organization, ApiOptions.None);
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user for a given organization for the authenticated user.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="organization">The name of the organization</param>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOrganization(string organization, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(organization, "organization");
            Ensure.ArgumentNotNull(options, "options");

            return GetAllForOrganization(organization, new IssueRequest(), options);
        }

        /// <summary>
        /// Gets all issues for a given organization for the authenticated user.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="organization">The name of the organization</param>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOrganization(string organization, IssueRequest request)
        {
            Ensure.ArgumentNotNullOrEmptyString(organization, "organization");
            Ensure.ArgumentNotNull(request, "request");

            return GetAllForOrganization(organization, request, ApiOptions.None);
        }

        /// <summary>
        /// Gets all issues for a given organization for the authenticated user.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues
        /// </remarks>
        /// <param name="organization">The name of the organization</param>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForOrganization(string organization, IssueRequest request, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(organization, "organization");
            Ensure.ArgumentNotNull(request, "request");
            Ensure.ArgumentNotNull(options, "options");

            return _connection.GetAndFlattenAllPages<Issue>(ApiUrls.Issues(organization), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user for the repository.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues-for-a-repository
        /// </remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForRepository(string owner, string name)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
            Ensure.ArgumentNotNullOrEmptyString(name, "name");

            return GetAllForRepository(owner, name, ApiOptions.None);
        }

        /// <summary>
        /// Gets all open issues assigned to the authenticated user for the repository.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues-for-a-repository
        /// </remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForRepository(string owner, string name, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
            Ensure.ArgumentNotNullOrEmptyString(name, "name");
            Ensure.ArgumentNotNull(options, "options");

            return GetAllForRepository(owner, name, new RepositoryIssueRequest(), options);
        }

        /// <summary>
        /// Gets issues for a repository.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues-for-a-repository
        /// </remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForRepository(string owner, string name, RepositoryIssueRequest request)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
            Ensure.ArgumentNotNullOrEmptyString(name, "name");
            Ensure.ArgumentNotNull(request, "request");

            return GetAllForRepository(owner, name, request, ApiOptions.None);
        }

        /// <summary>
        /// Gets issues for a repository.
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/issues/#list-issues-for-a-repository
        /// </remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="request">Used to filter and sort the list of issues returned</param>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>A signal containing one or more <see cref="Issue"/>s.</returns>
        public IObservable<Issue> GetAllForRepository(string owner, string name, RepositoryIssueRequest request, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
            Ensure.ArgumentNotNullOrEmptyString(name, "name");
            Ensure.ArgumentNotNull(request, "request");
            Ensure.ArgumentNotNull(options, "options");

            return _connection.GetAndFlattenAllPages<Issue>(ApiUrls.Issues(owner, name), request.ToParametersDictionary(), options);
        }

        /// <summary>
        /// Creates an issue for the specified repository. Any user with pull access to a repository can create an
        /// issue.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/issues/#create-an-issue</remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="newIssue">A <see cref="NewIssue"/> instance describing the new issue to create</param>
        /// <returns>A signal containing the new <see cref="Issue"/>.</returns>
        public IObservable<Issue> Create(string owner, string name, NewIssue newIssue)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
            Ensure.ArgumentNotNullOrEmptyString(name, "name");
            Ensure.ArgumentNotNull(newIssue, "newIssue");

            return _client.Create(owner, name, newIssue).ToObservable();
        }

        /// <summary>
        /// Creates an issue for the specified repository. Any user with pull access to a repository can create an
        /// issue.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/issues/#create-an-issue</remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="number">The issue number</param>
        /// <param name="issueUpdate">An <see cref="IssueUpdate"/> instance describing the changes to make to the issue
        /// </param>
        /// <returns>A signal containing the updated <see cref="Issue"/>.</returns>
        public IObservable<Issue> Update(string owner, string name, int number, IssueUpdate issueUpdate)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
            Ensure.ArgumentNotNullOrEmptyString(name, "name");
            Ensure.ArgumentNotNull(issueUpdate, "issueUpdate");

            return _client.Update(owner, name, number, issueUpdate).ToObservable();
        }

        /// <summary>
        /// Locks an issue for the specified repository. Issue owners and users with push access can lock an issue.
        /// </summary>
        /// <remarks>https://developer.github.com/v3/issues/#lock-an-issue</remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="number">The issue number</param>
        /// <returns>A signal indicating completion.</returns>
        public IObservable<Unit> Lock(string owner, string name, int number)
         {
             Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
             Ensure.ArgumentNotNullOrEmptyString(name, "name");
 
             return _client.Lock(owner, name, number).ToObservable();
         }

        /// <summary>
        /// Unlocks an issue for the specified repository. Issue owners and users with push access can unlock an issue.
        /// </summary>
        /// <remarks>https://developer.github.com/v3/issues/#unlock-an-issue</remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="number">The issue number</param>
        /// <returns>A signal indicating completion.</returns>
        public IObservable<Unit> Unlock(string owner, string name, int number)
         {
             Ensure.ArgumentNotNullOrEmptyString(owner, "owner");
             Ensure.ArgumentNotNullOrEmptyString(name, "name");
 
             return _client.Unlock(owner, name, number).ToObservable();
         }
    }
}
