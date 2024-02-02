//
//  User.swift
//  Achiever
//
//  Created by Tom Brewer on 1/31/24.
//

import Foundation

struct UserInfo: Codable {
    let email: String
    let isEmailConfirmed: Bool
    let claims: [String: String]?
}
