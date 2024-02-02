//
//  LoginRequest.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

struct LoginRequest: Codable {
    let email: String
    let password: String
    let twoFactorCode: String?
    let twoFactorRecoveryCode: String?
}
