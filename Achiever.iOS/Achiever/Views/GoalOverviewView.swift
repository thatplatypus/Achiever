//
//  GoalOverviewView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct GoalOverviewView: View {
    @State private var goals = [Goal]()
    @State private var isLoading = false
    let goalClient = GoalClient(networkManager: NetworkManager())

    var body: some View {
        VStack {
            if isLoading {
                ProgressView("Loading...")
            } else {
                ScrollView {
                    LazyVGrid(columns: [GridItem(.adaptive(minimum: 200))]) {
                        ForEach(goals) { goal in
                            GoalCardView(goal: goal)
                        }
                    }
                }
            }
        }
        .onAppear(perform: loadGoals)
        .navigationBarTitle("Goals", displayMode: .inline)
        .navigationBarItems(trailing: Button(action: logout) {
            Text("Logout")
        })
    }
    
    func loadGoals() {
        isLoading = true
        goalClient.fetchGoals { result in
            isLoading = false
            switch result {
            case .success(let fetchedGoals):
                goals = fetchedGoals
            case .failure(let error):
                print("Failed to fetch goals: \(error)")
            }
        }
    }
    
    func logout() {
        // Implement logout functionality
    }
}
