//
//  SignInWithAppleView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/4/24.
//

import Foundation
import SwiftUI
import AuthenticationServices

struct SignInWithAppleView: View {
    var body: some View {
        SignInWithAppleButton(.signIn, onRequest: { request in
            request.requestedScopes = [.fullName, .email]
        }, onCompletion: { result in
            switch result {
            case .success(let authResults):
                // Extract the user identifier and the identity token
                if let appleIDCredential = authResults.credential as? ASAuthorizationAppleIDCredential,
                   let identityTokenData = appleIDCredential.identityToken,
                   let identityToken = String(data: identityTokenData, encoding: .utf8) {
                    
                    // Send the user identifier and the identity token to your backend
                    let userIdentifier = appleIDCredential.user
                    sendToBackend(userIdentifier: userIdentifier, identityToken: identityToken)
                }
            case .failure(let error):
                print("Authorization failed: " + error.localizedDescription)
            }
        })
        .frame(width: 280, height: 60)
    }
    
    func sendToBackend(userIdentifier: String, identityToken: String) {
        // Create your request
        let url = URL(string: "https://your-backend.com/auth")!
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.httpBody = "userIdentifier=\(userIdentifier)&identityToken=\(identityToken)".data(using: .utf8)
        
        // Send the request
        URLSession.shared.dataTask(with: request) { data, response, error in
            if let error = error {
                print("Failed to authenticate with backend: " + error.localizedDescription)
            } else if let data = data {
                // Handle the response from your backend
            }
        }.resume()
    }
}
