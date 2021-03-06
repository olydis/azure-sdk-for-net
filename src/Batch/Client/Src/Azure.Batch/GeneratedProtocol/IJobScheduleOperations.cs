// Copyright (c) Microsoft and contributors.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Batch.Protocol
{
    using Microsoft.Rest.Azure;
    using Models;

    /// <summary>
    /// JobScheduleOperations operations.
    /// </summary>
    public partial interface IJobScheduleOperations
    {
        /// <summary>
        /// Checks the specified job schedule exists.
        /// </summary>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule which you want to check.
        /// </param>
        /// <param name='jobScheduleExistsOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationResponse<bool,JobScheduleExistsHeaders>> ExistsWithHttpMessagesAsync(string jobScheduleId, JobScheduleExistsOptions jobScheduleExistsOptions = default(JobScheduleExistsOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Deletes a job schedule from the specified account.
        /// </summary>
        /// <remarks>
        /// When you delete a job schedule, this also deletes all jobs and
        /// tasks under that schedule. When tasks are deleted, all the files in
        /// their working directories on the compute nodes are also deleted
        /// (the retention period is ignored). The job schedule statistics are
        /// no longer accessible once the job schedule is deleted, though they
        /// are still counted towards account lifetime statistics.
        /// </remarks>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule to delete.
        /// </param>
        /// <param name='jobScheduleDeleteOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationHeaderResponse<JobScheduleDeleteHeaders>> DeleteWithHttpMessagesAsync(string jobScheduleId, JobScheduleDeleteOptions jobScheduleDeleteOptions = default(JobScheduleDeleteOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Gets information about the specified job schedule.
        /// </summary>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule to get.
        /// </param>
        /// <param name='jobScheduleGetOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationResponse<CloudJobSchedule,JobScheduleGetHeaders>> GetWithHttpMessagesAsync(string jobScheduleId, JobScheduleGetOptions jobScheduleGetOptions = default(JobScheduleGetOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Updates the properties of the specified job schedule.
        /// </summary>
        /// <remarks>
        /// This replaces only the job schedule properties specified in the
        /// request. For example, if the schedule property is not specified
        /// with this request, then the Batch service will keep the existing
        /// schedule. Changes to a job schedule only impact jobs created by the
        /// schedule after the update has taken place; currently running jobs
        /// are unaffected.
        /// </remarks>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule to update.
        /// </param>
        /// <param name='jobSchedulePatchParameter'>
        /// The parameters for the request.
        /// </param>
        /// <param name='jobSchedulePatchOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationHeaderResponse<JobSchedulePatchHeaders>> PatchWithHttpMessagesAsync(string jobScheduleId, JobSchedulePatchParameter jobSchedulePatchParameter, JobSchedulePatchOptions jobSchedulePatchOptions = default(JobSchedulePatchOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Updates the properties of the specified job schedule.
        /// </summary>
        /// <remarks>
        /// This fully replaces all the updateable properties of the job
        /// schedule. For example, if the schedule property is not specified
        /// with this request, then the Batch service will remove the existing
        /// schedule. Changes to a job schedule only impact jobs created by the
        /// schedule after the update has taken place; currently running jobs
        /// are unaffected.
        /// </remarks>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule to update.
        /// </param>
        /// <param name='jobScheduleUpdateParameter'>
        /// The parameters for the request.
        /// </param>
        /// <param name='jobScheduleUpdateOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationHeaderResponse<JobScheduleUpdateHeaders>> UpdateWithHttpMessagesAsync(string jobScheduleId, JobScheduleUpdateParameter jobScheduleUpdateParameter, JobScheduleUpdateOptions jobScheduleUpdateOptions = default(JobScheduleUpdateOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Disables a job schedule.
        /// </summary>
        /// <remarks>
        /// No new jobs will be created until the job schedule is enabled
        /// again.
        /// </remarks>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule to disable.
        /// </param>
        /// <param name='jobScheduleDisableOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationHeaderResponse<JobScheduleDisableHeaders>> DisableWithHttpMessagesAsync(string jobScheduleId, JobScheduleDisableOptions jobScheduleDisableOptions = default(JobScheduleDisableOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Enables a job schedule.
        /// </summary>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule to enable.
        /// </param>
        /// <param name='jobScheduleEnableOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationHeaderResponse<JobScheduleEnableHeaders>> EnableWithHttpMessagesAsync(string jobScheduleId, JobScheduleEnableOptions jobScheduleEnableOptions = default(JobScheduleEnableOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Terminates a job schedule.
        /// </summary>
        /// <param name='jobScheduleId'>
        /// The ID of the job schedule to terminates.
        /// </param>
        /// <param name='jobScheduleTerminateOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationHeaderResponse<JobScheduleTerminateHeaders>> TerminateWithHttpMessagesAsync(string jobScheduleId, JobScheduleTerminateOptions jobScheduleTerminateOptions = default(JobScheduleTerminateOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Adds a job schedule to the specified account.
        /// </summary>
        /// <param name='cloudJobSchedule'>
        /// The job schedule to be added.
        /// </param>
        /// <param name='jobScheduleAddOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationHeaderResponse<JobScheduleAddHeaders>> AddWithHttpMessagesAsync(JobScheduleAddParameter cloudJobSchedule, JobScheduleAddOptions jobScheduleAddOptions = default(JobScheduleAddOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Lists all of the job schedules in the specified account.
        /// </summary>
        /// <param name='jobScheduleListOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationResponse<Microsoft.Rest.Azure.IPage<CloudJobSchedule>,JobScheduleListHeaders>> ListWithHttpMessagesAsync(JobScheduleListOptions jobScheduleListOptions = default(JobScheduleListOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Lists all of the job schedules in the specified account.
        /// </summary>
        /// <param name='nextPageLink'>
        /// The NextLink from the previous successful call to List operation.
        /// </param>
        /// <param name='jobScheduleListNextOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.Azure.AzureOperationResponse<Microsoft.Rest.Azure.IPage<CloudJobSchedule>,JobScheduleListHeaders>> ListNextWithHttpMessagesAsync(string nextPageLink, JobScheduleListNextOptions jobScheduleListNextOptions = default(JobScheduleListNextOptions), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}
