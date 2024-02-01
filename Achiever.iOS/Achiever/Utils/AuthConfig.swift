//
//  AuthConfig.swift
//  Achiever
//
//  Created by Tom Brewer on 1/31/24.
//

import Foundation

struct AuthConfig {
    static let baseURL = Bundle.main.infoDictionary?["AuthBaseUrl"] as? String ?? "http://localhost:7211"
}
