//
//  GoalCardView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct GoalCardView: View {
    @ObservedObject var baseGoal: ObservableGoal = ObservableGoal();
    @State private var showingDetail = false
    @ObservedObject private var selectedSubTask = SelectedSubTask()
    @ObservedObject private var selectedGoal = ObservableGoal()
    @State private var scrollProgress: CGFloat = 0
    @AppStorage("viewType") var viewType: String?
    @AppStorage("isDarkMode") var isDarkMode: Bool = false
    
    var goal: Goal
    var goalUpdated: (Goal) -> Void = { _ in }

    let displayDateFormatter: DateFormatter = {
        let formatter = DateFormatter()
        formatter.dateStyle = .medium
        return formatter
    }()
    
    init(goal: Goal, onGoalUpdated: @escaping (Goal) -> Void = { _ in })
    {
        self.goal = goal
        self.baseGoal.goal = self.goal
        self.goalUpdated = onGoalUpdated
    }
    
    var body: some View {
        NavigationLink(destination: GoalDetailView(goal: goal, onGoalUpdated: goalUpdated)) {
            VStack(alignment: .leading) {
                HStack {
                    Text(baseGoal.goal.title!)
                    Spacer()
                    if let targetDate = baseGoal.goal.targetEndDate{
                        Text("Target: \(displayDateFormatter.string(from: targetDate))")
                            .font(.footnote)
                            .foregroundColor(.gray)
                        }
                }
                if let subTasks = baseGoal.goal.subTasks, !subTasks.isEmpty {
                    let completedTasks = subTasks.filter({ $0.status?.lowercased() == "completed" }).count
                    Text("\(completedTasks) / \(subTasks.count) Complete")
                        .font(.footnote)
                        .foregroundColor(.gray)
                    
                    VStack {
                        GeometryReader { geometry in
                            HStack {
                                ScrollView(.horizontal, showsIndicators: false) {
                                    HStack {
                                        ForEach(subTasks) { subTask in
                                            Button(action: {
                                                self.selectedGoal.goal = baseGoal.goal
                                                self.selectedSubTask.subTask = subTask
                                                self.showingDetail = true
                                            }) {
                                                    SubtaskCardView(title: subTask.title!,
                                                                    hours: subTask.estimatedHours ?? 0,
                                                                    status: subTask.status ?? "New",
                                                                    hasNotes: subTask.note != nil && subTask.note != "",
                                                                    color: getColorForStatus(subTask.status))
                                                    .padding(1)
                                                    .shadow(radius: 6)
                                            }
                                        }
                                    }
                                }
                                .frame(width: geometry.size.width * 0.7)
                                .cornerRadius(4)
                                .padding(2)
                                
                                let ratio = Double(completedTasks) / Double(subTasks.count)
                                DonutChartView(ratio: ratio, height: 75, width: 75)
                                    .frame(width: geometry.size.width * 0.3)
                            }
                        }
                        
                        Spacer()
                    }
                    .frame(minHeight: 100)
                }
                
                Spacer()
                
                if let totalHours = baseGoal.goal.subTasks?.reduce(0, { $0 + ($1.estimatedHours ?? 0) }), totalHours > 0 {
                    let completedHours = baseGoal.goal.subTasks?.filter({ $0.status?.lowercased() == "completed" }).reduce(0, { $0 + ($1.estimatedHours ?? 0) }) ?? 0
                    let remainingHours = totalHours - completedHours
                    HStack {
                        Image(systemName: "clock")
                            .foregroundColor(.gray)
                        Text("\(String(format: "%.2g", remainingHours))/\(String(format: "%.2g", totalHours)) hours remaining")
                            .font(.footnote)
                            .foregroundColor(.gray)
                    }
                    .padding(8)
                }
            }
            .padding()
            .background(Color(UIColor.secondarySystemBackground))
            .cornerRadius(10)
            .shadow(radius: 10)
            .id(viewType)
        }
        .preferredColorScheme(isDarkMode ? .dark : .light)
        .buttonStyle(PlainButtonStyle())
        .popover(isPresented: $showingDetail){
            if selectedSubTask.subTask.title != nil {
                SubtaskDetailModal(goal: $selectedGoal.goal, subTask: $selectedSubTask.subTask, onSave: { updatedGoal in
                    DispatchQueue.main.async {
                        baseGoal.goal = updatedGoal
                        let id = selectedSubTask.subTask.id
                        if let updatedSubTasks = baseGoal.goal.subTasks?.map({ $0.id == id ? selectedSubTask.subTask : $0 }) {
                            baseGoal.goal.subTasks = updatedSubTasks
                        }
                        goalUpdated(updatedGoal)
                    }
                })
            }
        }
    }
}

        
        
        func getColorForStatus(_ status: String?) -> Color {
            switch status?.lowercased() {
            case "new":
                return .gray
            case "inprogress":
                return .cyan
            case "completed":
                return .green
            default:
                return .gray
            }
        }
    
    class SelectedSubTask: ObservableObject {
        @Published var subTask: SubTask = SubTask()
    }

class ObservableGoal: ObservableObject {
    @Published var goal: Goal = Goal()
}

