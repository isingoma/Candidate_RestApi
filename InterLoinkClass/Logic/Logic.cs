using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterLoinkClass.Logic
{
    public class Logic
    {

        string connectionString = "Data Source=ANTHONY;Initial Catalog=PaymentsDatabase; Integrated Security=true\" providerName=\"System.Data.SqlClient";
        public StudentDetailsResponse CreateCandidate(StudentDetails dets, string ip)
        {
            StudentDetailsResponse response = new StudentDetailsResponse();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if candidate exists
                    string checkQuery = "SELECT COUNT(*) FROM Candidates WHERE email = @Email";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Email", dets.Email);
                        int existingCount = (int)checkCommand.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            response.statuscode = "100";
                            response.statusdescription = "Candidate with the provided email already exists.";
                            return response;
                        }
                    }

                    dets.CandidateId = GenerateCandidateId();

                    // Insert the details
                    string insertQuery = @"INSERT INTO Candidates (candidate_id, first_name, last_name, phone_number, email, call_time_interval, linkedin_url, github_url, comment)
                                VALUES (@CandidateId, @FirstName, @LastName, @PhoneNumber, @Email, @CallTimeInterval, @LinkedInUrl, @GitHubUrl, @Comment)";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@CandidateId", dets.CandidateId);
                        insertCommand.Parameters.AddWithValue("@FirstName", dets.FirstName);
                        insertCommand.Parameters.AddWithValue("@LastName", dets.LastName);
                        insertCommand.Parameters.AddWithValue("@PhoneNumber", dets.PhoneNumber);
                        insertCommand.Parameters.AddWithValue("@Email", dets.Email);
                        insertCommand.Parameters.AddWithValue("@CallTimeInterval", dets.CallTimeInterval);
                        insertCommand.Parameters.AddWithValue("@LinkedInUrl", dets.LinkedInUrl);
                        insertCommand.Parameters.AddWithValue("@GitHubUrl", dets.GitHubUrl);
                        insertCommand.Parameters.AddWithValue("@Comment", dets.Comment);

                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            response.statuscode = "0";
                            response.statusdescription = "Candidate details inserted successfully.";
                            response.statusdescription = dets.CandidateId;
                        }
                        else
                        {
                            response.statuscode = "100";
                            response.statusdescription = "Failed to insert candidate details.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.statuscode = "100";
                response.statusdescription = "An error occurred while inserting candidate details: " + ex.Message;
            }
            return response;
        }
        public string GenerateCandidateId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var candidateId = new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return candidateId;
        }
        public StudentDetailsResponse UpdateCandidate(StudentDetails dets, string ip)
        {
            StudentDetailsResponse response = new StudentDetailsResponse();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = @"UPDATE Candidates 
                                  SET github_url = @GitHubUrl, linkedin_url = @LinkedInUrl, phone_number = @PhoneNumber, call_time_interval = @CallTimeInterval
                                  WHERE email = @Email";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@GitHubUrl", dets.GitHubUrl);
                        command.Parameters.AddWithValue("@LinkedInUrl", dets.LinkedInUrl);
                        command.Parameters.AddWithValue("@PhoneNumber", dets.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", dets.Email);
                        command.Parameters.AddWithValue("@CallTimeInterval", dets.CallTimeInterval);
                        command.Parameters.AddWithValue("@CandidateId", dets.CandidateId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            response.statuscode = "0";
                            response.statusdescription = "Candidate details updated successfully.";
                            response.CandidateId = dets.CandidateId;
                        }
                        else
                        {
                            response.statuscode = "100";
                            response.statusdescription = "No candidate found with the provided email.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.statuscode = "100";
                response.statusdescription = "An error occurred while updating candidate details: " + ex.Message;
            }
            return response;
        }
        public StudentDetailsResponse QueryCandidate(StudentDetails dets, string ip)
        {
            StudentDetailsResponse response = new StudentDetailsResponse();
            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Candidates WHERE email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", dets.Email);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dt);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    response.statuscode = "0";
                    response.statusdescription = "SUCCESS";
                    response.fullname = dt.Rows[0]["first_name"].ToString().ToUpper() + " " + dt.Rows[0]["last_name"].ToString().ToUpper();
                    response.Email = dt.Rows[0]["email"].ToString();
                    response.CandidateId = dets.CandidateId;
                    response.GitHubUrl = dt.Rows[0]["github_url"].ToString();
                    response.LinkedInUrl = dt.Rows[0]["linkedin_url"].ToString();
                }
                else
                {
                    response.statuscode = "100";
                    response.statusdescription = "CANDIDATE WITH ID " + dets.CandidateId + " DOES NOT EXIST";
                }
            }
            catch (Exception ex)
            {
                response.statuscode = "100";
                response.statusdescription = "ERROR OCCURRED WHILE RETRIEVING DETAILS FOR CANDIDATE " + dets.Email;
            }
            return response;
        }
    }
}
