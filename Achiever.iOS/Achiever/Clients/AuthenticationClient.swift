//
//  AuthenticationClient.swift
//  Achiever
//
//  Created by Tom Brewer on 1/31/24.
//

import Foundation

class AuthenticationClient {
    let networkManager: NetworkManager

    init(networkManager: NetworkManager) {
        self.networkManager = networkManager
    }

    func login(username: String, password: String, completion: @escaping (Result<UserInfo, Error>) -> Void) {
        // Construct your login request
        var loginRequest = URLRequest(url: URL(string: AuthConfig.baseURL + "/login?useCookies=true")!)
        
        // Set the method to POST
        loginRequest.httpMethod = "POST"
        
        // Create the request body
        let body = ["email": username, "password": password]
        loginRequest.httpBody = try? JSONEncoder().encode(body)
        
        // Set the content type to JSON
        loginRequest.setValue("application/json", forHTTPHeaderField: "Content-Type")
        
        // Use the network manager to make the request
        networkManager.post(request: loginRequest) { result in
            switch result {
            case .success(let data):
                // Log the data
                print("Received data: \(data)")
                
                // Make a GET request to the manage/info endpoint
                var infoRequest = URLRequest(url: URL(string: AuthConfig.baseURL + "/manage/info")!)
                infoRequest.httpMethod = "GET"
                infoRequest.setValue("application/json", forHTTPHeaderField: "Content-Type")
                
                self.networkManager.get(request: infoRequest) { result in
                    switch result {
                    case .success(let data):
                        let jsonString = String(data: data, encoding: .utf8)
                            print("Received JSON: \(jsonString ?? "nil")")


                        // Decode the UserInfo from the response data
                        let decoder = JSONDecoder()
                        if let user = try? decoder.decode(UserInfo.self, from: data) {
                            completion(.success(user))
                        } else {
                            completion(.failure(NetworkError.decodingError))
                        }
                    case .failure(let error):
                        completion(.failure(error))
                    }
                }
            case .failure(let error):
                completion(.failure(error))
            }
        }
    }
}
