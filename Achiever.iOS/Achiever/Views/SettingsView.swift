//
//  SettingsView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/2/24.
//

import Foundation
import SwiftUI
import KeychainSwift

struct SettingsView: View {
    @Environment(\.colorScheme) var colorScheme
    @EnvironmentObject var userSettings: UserSettings
    @AppStorage("isDarkMode") var isDarkMode: Bool = false
    @AppStorage("userEmail") var storedEmail: String = ""
    let keychain = KeychainSwift()
    let authClient = AuthenticationClient(networkManager: NetworkManager())
    
    var body: some View {
        NavigationView {
            Form {
                Section(header: Text("User Information")) {
                    Text(userSettings.userEmail)
                }
                
                Section(header: Text("Settings")) {
                    Toggle(isOn: $isDarkMode) {
                        Text("Dark Mode")
                    }
                    
                }
                
                Section {
                    Button("Logout") {
                        authClient.logout { result in
                            switch result {
                            case .success:
                                // Clear the stored credentials
                                keychain.delete("email")
                                keychain.delete("password")

                                // Log out the user
                                userSettings.userEmail = "guest"
                            case .failure(let error):
                                // Handle logout error here
                                print("Failed to log out: \(error)")
                            }
                        }
                    }
                }
            }
            .navigationTitle("Settings")
            .preferredColorScheme(isDarkMode ? .dark : .light)
        }
    }
}
