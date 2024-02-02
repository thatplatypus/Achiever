//
//  GoalCardView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct GoalCardView: View {
    var goal: Goal

    var body: some View {
        VStack(alignment: .leading) {
            HStack {
                Text(goal.title!)
                Spacer()
                if let targetDate = goal.targetEndDate {
                    Text("Target: \(targetDate, formatter: DateFormatter())")
                        .font(.footnote)
                        .foregroundColor(.gray)
                }
            }
            if let subTasks = goal.subTasks, !subTasks.isEmpty {
                LazyVGrid(columns: Array(repeating: .init(.flexible()), count: 2), spacing: 10) {
                    ForEach(subTasks.prefix(4)) { subTask in
                        BadgeView(text: subTask.title!, color: getColorForStatus(subTask.status))
                    }
                }
            }
            if let totalHours = goal.subTasks?.reduce(0, { $0 + ($1.estimatedHours ?? 0) }), totalHours > 0 {
                let completedHours = goal.subTasks?.filter({ $0.status?.lowercased() == "completed" }).reduce(0, { $0 + ($1.estimatedHours ?? 0) }) ?? 0;  let remainingHours = totalHours - completedHours
                Text("Total hours remaining: \(String(format: "%.2f", remainingHours))/\(String(format: "%.2f", totalHours))")
                    .font(.footnote)
                    .foregroundColor(.gray)
            }
        }
        .padding()
        .background(Color.white)
        .cornerRadius(10)
        .shadow(radius: 10)
    }
    
    func getColorForStatus(_ status: String?) -> Color {
        switch status?.lowercased() {
        case "new":
            return .gray
        case "inprogress":
            return .blue
        case "completed":
            return .green
        default:
            return .gray
        }
    }
}
