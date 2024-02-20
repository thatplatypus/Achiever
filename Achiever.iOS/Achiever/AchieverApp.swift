//
//  AchieverApp.swift
//  Achiever
//
//  Created by Tom Brewer on 1/31/24.
//

import SwiftUI

@main
struct AchieverApp: App {
    @StateObject private var userSettings = UserSettings()

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environmentObject(userSettings)
        }
    }
}
