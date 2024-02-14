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
    @Published var sortOption: SortOption = .lastModified
    @Published var hideCompletedGoals: Bool = false
    @Published var hidePastGoals: Bool = false
    @Published var searchText: String = ""
    let goalClient = GoalClient(networkManager: NetworkManager())

    init() {
        loadGoals()
    }
    
    var filteredGoals: [Goal] {
        var filtered = goals
        if(hideCompletedGoals) {
            filtered = filtered.filter { goal in
                !(goal.subTasks!.allSatisfy { $0.status?.lowercased() == "completed" } && !goal.subTasks!.isEmpty)
            }
        }
        
        if(hidePastGoals) {
            let currentDate = Date()
            filtered = filtered.filter { goal in
                guard let targetEndDate = goal.targetEndDate else {
                    return false // Exclude goals with nil end date
                }
                return targetEndDate >= currentDate // Only include goals with end date in the future
            }
        }
        
           if searchText.isEmpty {
               return filtered
           } else {
               return filtered.filter { goal in
                   goal.title!.contains(searchText)
               }
           }
       }

    func loadGoals(refresh: Bool = false) {
    isLoading = true
    goalClient.fetchGoals { result in
        DispatchQueue.main.asyncAfter(deadline: .now() + (refresh ? 1.0 : 0)) {
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
}

enum SortOption {
    case lastModified, title, targetDate
}
