//
//  NetworkError.swift
//  Achiever
//
//  Created by Tom Brewer on 1/31/24.
//

import Foundation

enum NetworkError: Error {
    case urlError
    case decodingError
    case networkRequestFailed
    case invalidURL
}
