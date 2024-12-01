
using Microsoft.Data.SqlClient;
using System.Data;
using Xunit;

namespace BlogPostTests
{
    public class BlogPostDatabaseTests
    {
        private const string ConnectionString = "Server=.;Database=master;User Id=sa;Password=123;TrustServerCertificate=True;";

        [Fact]
        public void TestDatabaseSetupAndOperations()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Step 1: Setup Database
                ExecuteNonQuery(connection, @"
                        USE master;

                        -- Close open connections and delete the database
                        IF DB_ID('TestBlogDB') IS NOT NULL
                        BEGIN
                            ALTER DATABASE TestBlogDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                            DROP DATABASE TestBlogDB;
                        END;

                        -- Create new database
                        CREATE DATABASE TestBlogDB;
                    ");

                ExecuteNonQuery(connection, @"
                        -- Use the new database
                        USE TestBlogDB;

                        -- Table of Topical Headings
                        CREATE TABLE MainTopics
                        (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Title NVARCHAR(255) NOT NULL
                        );
                    ");

                ExecuteNonQuery(connection, @"
                        -- Blog Posts Table
                        CREATE TABLE BlogPosts
                        (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Title NVARCHAR(255) NOT NULL,
                            Content NVARCHAR(MAX) NOT NULL,
                            MainTopicId INT NOT NULL,
                            FOREIGN KEY (MainTopicId) REFERENCES MainTopics(Id)
                        );
                    ");

                ExecuteNonQuery(connection, @"
                        -- Blog addition Stored Procedure
                        CREATE PROCEDURE InserOrUpdateBlogPost
                            @Id INT,
                            @Title NVARCHAR(255),
                            @Content NVARCHAR(MAX),
                            @MainTopicId INT,
                            @StatusCode INT OUTPUT
                        AS
                        BEGIN
                            -- MainTopicId validation
                            IF NOT EXISTS (SELECT * FROM MainTopics WITH(NOLOCK) WHERE Id = @MainTopicId)
                            BEGIN
                                SET @StatusCode = 2; -- No parent topic title found
                                RETURN;
                            END;

                            IF EXISTS (SELECT * FROM BlogPosts WITH(NOLOCK) WHERE Id = @Id)
                            BEGIN
                                -- Updated a blog post
                                UPDATE BlogPosts 
                                SET Title = @Title, Content = @Content, MainTopicId = @MainTopicId
                                WHERE Id = @Id;

                                SET @StatusCode = 1; -- Blog successfully Updated
                                RETURN;
                            END
                            ELSE 
                            BEGIN
                                -- Adding a blog post
                                INSERT INTO BlogPosts (Title, Content, MainTopicId)
                                VALUES (@Title, @Content, @MainTopicId);

                                SET @StatusCode = 0; -- Blog successfully added
                                RETURN;
                            END;
                        END;
                    ");

                ExecuteNonQuery(connection, @"
                        -- Add sample data
                        INSERT INTO MainTopics (Title) VALUES ('Technology');
                        INSERT INTO MainTopics (Title) VALUES ('Lifestyle');
                    ");

                // Step 2: Insert a new blog post
                int statusCode;
                ExecuteStoredProcedure(connection, "TestBlogDB", "InserOrUpdateBlogPost", 0, "First Post", "This is the content of the first post.", 1, out statusCode);
                Assert.Equal(0, statusCode); // Blog successfully added

                // Step 3: Update the blog post
                ExecuteStoredProcedure(connection, "TestBlogDB", "InserOrUpdateBlogPost", 1, "Updated Post", "This is the updated content.", 1, out statusCode);
                Assert.Equal(1, statusCode); // Blog successfully updated

                // Step 4: Attempt to insert a blog post with a non-existent MainTopicId
                ExecuteStoredProcedure(connection, "TestBlogDB", "InserOrUpdateBlogPost", 0, "Invalid Post", "This post has an invalid MainTopicId.", 999, out statusCode);
                Assert.Equal(2, statusCode); // No parent topic title found
            }
        }

        private void ExecuteNonQuery(SqlConnection connection, string commandText)
        {
            using (var command = new SqlCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void ExecuteStoredProcedure(SqlConnection connection, string database, string procedureName, int id, string title, string content, int mainTopicId, out int statusCode)
        {
            using (var command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Content", content);
                command.Parameters.AddWithValue("@MainTopicId", mainTopicId);
                var statusCodeParam = new SqlParameter("@StatusCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(statusCodeParam);

                command.ExecuteNonQuery();
                statusCode = (int)statusCodeParam.Value;
            }
        }
    }
}
