//
//  GoalData.swift
//  Achiever
//
//  Created by Tom Brewer on 2/2/24.
//

import Foundation
class GoalData: ObservableObject {
    @Published var goals = [Goal]()
    @Published var isLoading = false
    let goalClient = GoalClient(networkManager: NetworkManager())

    init() {
        loadGoals()
    }

    func loadGoals() {
        isLoading = true
        goalClient.fetchGoals { result in
            self.isLoading = false
            switch result {
            case .success(let fetchedGoals):
                self.goals = fetchedGoals
            case .failure(let error):
                print("Failed to fetch goals: \(error)")
            }
        }
    }
}
