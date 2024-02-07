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
    @AppStorage("viewType") var viewType: String = "card"
    @State var isBadgeDisplay: Bool
    
    init() {
            _isBadgeDisplay = State(initialValue: UserDefaults.standard.string(forKey: "viewType") == "badge")
        }
    
    var body: some View {
        NavigationView {
            Form {
                Section(header: Text("User Information")) {
                    // Display user information here
                }
                
                Section(header: Text("Settings")) {
                    Toggle(isOn: $isDarkMode) {
                        Text("Dark Mode")
                    }
                    
                    Toggle(isOn: $isBadgeDisplay) {
                                            Text("Use badge view")
                                        }
                                        .onChange(of: isBadgeDisplay) { newValue in
                                            viewType = newValue ? "badge" : "card"
                                            print(viewType)
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
