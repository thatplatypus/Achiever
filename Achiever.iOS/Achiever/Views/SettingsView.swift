//
//  SettingsView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/2/24.
//

import Foundation
import SwiftUI

struct SettingsView: View {
    @Environment(\.colorScheme) var colorScheme
    @AppStorage("isDarkMode") var isDarkMode: Bool = false
    @AppStorage("userEmail") var storedEmail: String = ""
    
    var body: some View {
        NavigationView {
            Form {
                Section(header: Text("User Information")) {
                    Text(storedEmail)
                }
                
                Section(header: Text("Settings")) {
                    Toggle(isOn: $isDarkMode) {
                        Text("Dark Mode")
                    }
                    
                }
                
                Section {
                    Button("Logout") {
                        // Handle logout here
                    }
                }
            }
            .navigationTitle("Settings")
            .preferredColorScheme(isDarkMode ? .dark : .light)
        }
    }
}
