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
                dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ssZ"
                decoder.dateDecodingStrategy = .formatted(dateFormatter)
                
                do {
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

    func getGoalById(id: UUID, completion: @escaping (Result<Goal, Error>) -> Void) {
        do {
            let requestObject = ["Id": id.uuidString]
            let requestData = try JSONSerialization.data(withJSONObject: requestObject)
            guard let requestString = String(data: requestData, encoding: .utf8) else {
                completion(.failure(NetworkError.invalidURL))
                return
            }

            guard let url = URL(string: AuthConfig.baseURL + "/GetGoalById?request=\(requestString)") else {
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
                dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.ssZ"
                decoder.dateDecodingStrategy = .formatted(dateFormatter)
                
                do {
                    let response = try decoder.decode(GetGoalByIdResponse.self, from: data)
                    completion(.success(response.goal))
                } catch {
                    completion(.failure(error))
                }
            case .failure(let error):
                completion(.failure(error))
            }
        }
        } catch {
            completion(.failure(NetworkError.networkRequestFailed))
        }
    }

    func createGoal(goal: Goal, completion: @escaping (Result<UUID, Error>) -> Void) {
        guard let url = URL(string: AuthConfig.baseURL + "/CreateGoal") else {
            completion(.failure(NetworkError.invalidURL))
            return
        }

        var request = URLRequest(url: url)
        let requestBody = CreateGoalRequest(goal: goal)
        request.httpMethod = "POST"
        request.httpBody = try? JSONEncoder().encode(requestBody)

        networkManager.send(request: request) { result in
            switch result {
            case .success(let data):
                do {
                    let decoder = JSONDecoder()
                    let response = try decoder.decode(CreateGoalResponse.self, from: data)
                    completion(.success(response.id))
                }
                catch {
                    completion(.failure(error))
                }
            case .failure(let error):
                completion(.failure(error))
            }
        }
    }

    func updateGoal(goal: Goal, completion: @escaping (Result<Bool, Error>) -> Void) {
    guard let url = URL(string: AuthConfig.baseURL + "/UpdateGoal") else {
        completion(.failure(NetworkError.invalidURL))
        return
    }

    var request = URLRequest(url: url)
        let goalWrapper = GoalWrapper(goal: goal)
        let encoder = JSONEncoder()
        encoder.dateEncodingStrategy = .iso8601
        let encodedGoal = try? encoder.encode(goalWrapper)
        let jsonString = String(data: encodedGoal!, encoding: .utf8) ?? ""

        request.httpBody = jsonString.data(using: .utf8)
        
        networkManager.send(request: request, method: "PUT") { result in
            switch result {
            case .success(let data):
                print(data)
                return completion(.success(true))
                let decoder = JSONDecoder()
                do {
                    let response = try decoder.decode(UpdateGoalResponse.self, from: data)
                    let isValidUUID = response.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000")
                    completion(.success(isValidUUID))
                } catch {
                    completion(.failure(error))
                }
            case .failure(let error):
                print(error)
                completion(.failure(error))
            }
        }
}
    
    func deleteGoal(id: UUID, completion: @escaping(Result<Bool, Error>) -> Void){
        guard let url = URL(string: AuthConfig.baseURL + "/DeleteGoal") else {
            completion(.failure(NetworkError.invalidURL))
            return
        }
        
        var request = URLRequest(url: url)
        var requestBody = DeleteGoalRequest(goalId: id)
        request.httpMethod = "POST"
        request.httpBody = try? JSONEncoder().encode(requestBody)

        networkManager.send(request: request) { result in
            switch result {
            case .success(let data):
                completion(.success(true))
            case .failure(let error):
                completion(.failure(error))
            }
        }
    }
    
    func deleteSubtask(id: UUID, completion: @escaping(Result<Bool, Error>) -> Void){
        guard let url = URL(string: AuthConfig.baseURL + "/DeleteSubtask") else {
            completion(.failure(NetworkError.invalidURL))
            return
        }
        
        var request = URLRequest(url: url)
        var requestBody = DeleteSubtaskRequest(subtaskId: id)
        request.httpMethod = "POST"
        request.httpBody = try? JSONEncoder().encode(requestBody)

        networkManager.send(request: request) { result in
            switch result {
            case .success(let data):
                completion(.success(true))
            case .failure(let error):
                completion(.failure(error))
            }
        }
    }
}

struct CreateGoalRequest: Codable {
    let goal: Goal
}

struct CreateGoalResponse: Codable {
    let id: UUID
}

struct GetGoalsResponse: Codable {
    let goals: [Goal]
}

struct GetGoalByIdResponse: Codable {
    let goal: Goal
}

struct UpdateGoalResponse: Codable {
    let id: UUID
}

struct DeleteGoalRequest: Codable {
    let goalId: UUID
}

struct DeleteSubtaskRequest: Codable {
    let subtaskId: UUID
}

struct GoalWrapper: Codable {
    let goal: Goal
}
