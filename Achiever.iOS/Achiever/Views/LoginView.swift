//
//  LoginView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct LoginView: View {
    let authClient = AuthenticationClient(networkManager: NetworkManager())
    @State private var email = "system@localhost"
    @State private var password = "Test123!"
    @Binding var userEmail: String

    var body: some View {
        VStack {
            TextField("Email", text: $email)
                .autocapitalization(.none)
                .keyboardType(.emailAddress)
                .padding()
            SecureField("Password", text: $password)
                .padding()
            Button(action: login) {
                Text("Log In")
            }
            .padding()
        }
    }
    
    func login() {
        authClient.login(username: email, password: password) { result in
            switch result {
            case .success(let userInfo):
                userEmail = userInfo.email
            case .failure(let error):
                print("Failed to log in: \(error)")
            }
        }
    }
}
