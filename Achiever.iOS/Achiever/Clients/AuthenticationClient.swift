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
        let loginRequest = URLRequest(url: URL(string: AuthConfig.baseURL + "/login?useCookies=true")!)
        
        // Use the network manager to make the request
        networkManager.post(request: loginRequest) { result in
            switch result {
            case .success(let data):
                // Decode the User from the response data
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
