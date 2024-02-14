//
//  NetworkManager.swift
//  Achiever
//
//  Created by Tom Brewer on 1/31/24.
//

import Foundation

class NetworkManager {
    
    init() {
    }
    
    func get(request: URLRequest, completion: @escaping (Result<Data, Error>) -> Void) {
        let task = URLSession.shared.dataTask(with: request) { (data, response, error) in
            if let error = error {
                completion(.failure(error))
            } else if let data = data {
                completion(.success(data))
            }
        }
        task.resume()
    }
    
    func send(request: URLRequest, method: String = "POST", completion: @escaping (Result<Data, Error>) -> Void) {
    var request = request
    request.httpMethod = method
    request.setValue("application/json", forHTTPHeaderField: "Content-Type")
    
    let task = URLSession.shared.dataTask(with: request) { (data, response, error) in
    if let error = error {
        completion(.failure(error))
    } else if let httpResponse = response as? HTTPURLResponse, 200..<300 ~= httpResponse.statusCode, let data = data {
        completion(.success(data))
    } else {
        let error = NSError(domain: "", code: (response as? HTTPURLResponse)?.statusCode ?? 500, userInfo: nil)
        completion(.failure(error))
    }
}
    task.resume()
}
}
