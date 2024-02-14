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
    @AppStorage("userEmail") var storedEmail: String = ""
    @AppStorage("isDarkMode") var isDarkMode: Bool = false
    @Binding var userEmail: String

    var body: some View {
        VStack {
            Text("Achiever")
                           .font(.largeTitle)
                           .fontWeight(.bold)
                           .padding()
            Spacer()
            TextField("Email", text: $email)
                .autocapitalization(.none)
                .keyboardType(.emailAddress)
                .padding()
            SecureField("Password", text: $password)
                .padding()
            Button(action: login) {
                Text("Log In")
                    .bold()
            }
            .padding()
            Button(action: register) {
                Text("Register")
                    .foregroundColor(.secondary)
            }
            .padding()
            Button(action: loginWithApple) {
                Text("Log In with Apple")
                    .foregroundColor(.black)
            }
            .padding()
            Spacer()
        }
        .preferredColorScheme(isDarkMode ? .dark : .light)
    }
    
    func login() {
        authClient.login(username: email, password: password) { result in
            switch result {
            case .success(let userInfo):
                userEmail = userInfo.email
                storedEmail = userEmail
            case .failure(let error):
                print("Failed to log in: \(error)")
            }
        }
    }

    func register() {
        // Implement your registration logic here
    }

    func loginWithApple() {
        // Implement your "Log In with Apple" logic here
    }
}
