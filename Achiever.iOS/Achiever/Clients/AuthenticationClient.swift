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
        networkManager.send(request: loginRequest) { result in
            switch result {
            case .success(let data):
                 self.getAuthenticatedInfo(completion: completion)

            case .failure(let error):
                completion(.failure(error))
            }
        }
    }
    
    func register(username: String, password: String, completion: @escaping (Result<UserInfo, Error>) -> Void) {
        // Construct your login request
        var registerRequest = URLRequest(url: URL(string: AuthConfig.baseURL + "/register")!)
        
        // Set the method to POST
        registerRequest.httpMethod = "POST"
        
        // Create the request body
        let body = ["email": username, "password": password]
        registerRequest.httpBody = try? JSONEncoder().encode(body)
        
        // Set the content type to JSON
        registerRequest.setValue("application/json", forHTTPHeaderField: "Content-Type")

            networkManager.send(request: registerRequest) { result in
                switch result {
                case .success:
                    self.getAuthenticatedInfo(completion: completion)
                case .failure(let error):
                    completion(.failure(error))
                }
            }
        }
    
    func logout(completion: @escaping (Result<Void, Error>) -> Void) {
        var logoutRequest = URLRequest(url: URL(string: AuthConfig.baseURL + "/Logout")!)
        logoutRequest.httpMethod = "POST"
        logoutRequest.setValue("application/json", forHTTPHeaderField: "Content-Type")
        logoutRequest.httpBody = try? JSONEncoder().encode([String: String]())

        networkManager.send(request: logoutRequest) { result in
            switch result {
            case .success:
                completion(.success(()))
            case .failure(let error):
                completion(.failure(error))
            }
        }
    }
    
    func getAuthenticatedInfo(completion: @escaping (Result<UserInfo, Error>) -> Void) {
            var infoRequest = URLRequest(url: URL(string: AuthConfig.baseURL + "/manage/info")!)
            infoRequest.httpMethod = "GET"
            infoRequest.setValue("application/json", forHTTPHeaderField: "Content-Type")

            networkManager.get(request: infoRequest) { result in
                switch result {
                case .success(let data):
                    let jsonString = String(data: data, encoding: .utf8)
                    print("Received JSON: \(jsonString ?? "nil")")

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
        }
}
