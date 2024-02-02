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
    
    func post(request: URLRequest, completion: @escaping (Result<Data, Error>) -> Void) {
          var request = request
          request.httpMethod = "POST"
          
          let task = URLSession.shared.dataTask(with: request) { (data, response, error) in
              if let error = error {
                  completion(.failure(error))
              } else if let data = data {
                  completion(.success(data))
              }
          }
          task.resume()
      }
}
