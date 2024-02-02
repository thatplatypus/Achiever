//
//  GoalClient.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation

struct GoalClient {
    let networkManager: NetworkManager

    func fetchGoals(completion: @escaping (Result<[Goal], Error>) -> Void) {
        guard let url = URL(string: AuthConfig.baseURL + "/GetGoals") else {
            completion(.failure(NetworkError.invalidURL))
            return
        }

        var request = URLRequest(url: url)
        request.httpMethod = "GET"

        networkManager.get(request: request) { result in
            switch result {
            case .success(let data):
                let decoder = JSONDecoder()
                let dateFormatter = DateFormatter()
                dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSSSSSZ"
                decoder.dateDecodingStrategy = .formatted(dateFormatter)
                
                do {
                    let jsonString = String(data: data, encoding: .utf8)
                        print("Received JSON: \(jsonString ?? "nil")")
                    
                    let response = try decoder.decode(GetGoalsResponse.self, from: data)
                    completion(.success(response.goals))
                } catch {
                    completion(.failure(error))
                }
            case .failure(let error):
                completion(.failure(error))
            }
        }
    }
}

struct GetGoalsResponse: Codable {
    let goals: [Goal]
}
