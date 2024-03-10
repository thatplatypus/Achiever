//
//  LoginView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI
import KeychainSwift

struct LoginView: View {
    let authClient = AuthenticationClient(networkManager: NetworkManager())
    let keychain = KeychainSwift()
    @State private var email = ""
    @State private var password = ""
    @AppStorage("userEmail") var storedEmail: String = ""
    @AppStorage("isDarkMode") var isDarkMode: Bool = false
    @EnvironmentObject var userSettings: UserSettings
    @State private var isLoading = false
    @State private var showingError = false
        @State private var errorMessage = ""

    var body: some View {
        VStack {
            if isLoading {
                ProgressView("Logging in...")
            } else {
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
            }
        }
        .preferredColorScheme(isDarkMode ? .dark : .light)
        .onAppear(perform: autoLogin)
    }
    
        func autoLogin() {
           
            if let storedEmail = keychain.get("email"),
               let storedPassword = keychain.get("password"),
               !storedEmail.isEmpty,
               !storedPassword.isEmpty {
                email = storedEmail
                password = storedPassword
                login()
            }
        }
     
    
    func login() {
        isLoading = true
        authClient.login(username: email, password: password) { result in
            switch result {
            case .success(let userInfo):
                userSettings.userEmail = userInfo.email
                storedEmail = userSettings.userEmail
                keychain.set(email, forKey: "email")
                keychain.set(password, forKey: "password")
            case .failure(let error):
                print("Failed to log in: \(error)")
            }
            isLoading = false
        }
    }

    func register() {
            authClient.register(username: email, password: password) { result in

                switch result {
                            case .success:
                                print("Registered \(email) successfully!")
                                keychain.set(email, forKey: "email")
                                keychain.set(password, forKey: "password")
                                login()
                            case .failure(let error):
                                // Handle registration error here
                                errorMessage = error.localizedDescription
                                showingError = true
                                
                                // Hide the error message after 3 seconds
                                DispatchQueue.main.asyncAfter(deadline: .now() + 3) {
                                    showingError = false
                                }
                            }
            }
    }

    func loginWithApple() {
        // Implement your "Log In with Apple" logic here
    }
    
    
}
