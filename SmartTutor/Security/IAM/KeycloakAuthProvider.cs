using SmartTutor.LearnerModel.Learners;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartTutor.Security.IAM
{
    public class KeycloakAuthProvider : IAuthProvider
    {
        private readonly string _loginPath =
            System.Environment.GetEnvironmentVariable("KEYCLOAK_LOGIN_PATH") ??
            "http://localhost:8080/auth/realms/master/protocol/openid-connect/token";

        private readonly string _registerPath =
            System.Environment.GetEnvironmentVariable("KEYCLOAK_REGISTER_PATH") ??
            "http://localhost:8080/auth/admin/realms/master/users";

        private readonly string _allUsersPath =
            System.Environment.GetEnvironmentVariable("KEYCLOAK_ALL_USERS_PATH") ??
            "http://localhost:8080/auth/admin/realms/master/users";

        private readonly string _adminUsername =
            EnvironmentConnection.GetSecret("KEYCLOAK_ADMIN_USERNAME") ?? "admin";

        private readonly string _adminPassword =
            EnvironmentConnection.GetSecret("KEYCLOAK_ADMIN_PASSWORD") ?? "admin";

        public async Task<Learner> Register(Learner learner)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"),
                _registerPath);

            //TODO: Take password from Learner this is just POC.
            request.Headers.TryAddWithoutValidation("Authorization", "bearer " + await AuthenticateAdmin());
            request.Content = new StringContent(
                $"{{\"enabled\":\"true\", \"username\":\"{learner.StudentIndex}\",\"credentials\":[{{\"type\":\"password\",\"value\":\"123\",\"temporary\":false}}]}}}}");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            await httpClient.SendAsync(request);

            return await SetIamIdToLearner(learner);
        }

        private async Task<string> AuthenticateAdmin()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), _loginPath);

            var contentList = new List<string>();
            contentList.Add("client_id=admin-cli");
            contentList.Add("username=" + _adminUsername);
            contentList.Add("password=" + _adminPassword);
            contentList.Add("grant_type=password");

            request.Content = new StringContent(string.Join("&", contentList));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            return await ExtractTokenFromResponse(httpClient, request);
        }

        private static async Task<string> ExtractTokenFromResponse(HttpClient httpClient, HttpRequestMessage request)
        {
            using var response = await httpClient.SendAsync(request);
            using var content = response.Content;
            return content.ReadAsStringAsync().Result.Split(":")[1].Split(",")[0].Split("\"")[1];
        }

        private async Task<string> GetAllUsers()
        {
            var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), _allUsersPath);

            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + await AuthenticateAdmin());
            request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
            using var response = await httpClient.SendAsync(request);
            using var content = response.Content;

            return content.ReadAsStringAsync().Result;
        }

        private async Task<Learner> SetIamIdToLearner(Learner learner)
        {
            var keycloakUsers = JsonSerializer.Deserialize<List<User>>(await GetAllUsers());
            foreach (var user in keycloakUsers.Where(s => s.Username.Equals(learner.StudentIndex)))
            {
                learner.IamId = user.Id;
            }

            return learner;
        }
    }
}