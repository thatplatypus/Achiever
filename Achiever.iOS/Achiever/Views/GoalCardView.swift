//
//  GoalCardView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct GoalCardView: View {
    var goal: Goal;
    @State private var showingDetail = false
    @ObservedObject private var selectedSubTask = SelectedSubTask()
    @State private var selectedGoal: Goal? = nil
    @State private var scrollProgress: CGFloat = 0
    @AppStorage("viewType") var viewType: String?
    @AppStorage("isDarkMode") var isDarkMode: Bool = false
    
    
    let displayDateFormatter: DateFormatter = {
        let formatter = DateFormatter()
        formatter.dateStyle = .medium
        return formatter
    }()
    
    var body: some View {
        NavigationLink(destination: GoalDetailView(goal: goal)) {
            let selectedGoal = goal
            VStack(alignment: .leading) {
                HStack {
                        Text(goal.title!)
                        Spacer()
                        if let targetDate = goal.targetEndDate {
                            Text("Target: \(displayDateFormatter.string(from: targetDate))")
                                .font(.footnote)
                                .foregroundColor(targetDate < Date() && ((goal.subTasks?.contains(where: { $0.status != "Completed" })) != nil) ? .red : .gray)
                        }
                    }
                if let subTasks = goal.subTasks, !subTasks.isEmpty {
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
                                                    self.selectedSubTask.subTask = subTask
                                                    self.showingDetail = true
                                            }) {
                                                if(viewType == "badge")
                                                {
                                                    BadgeView(text: subTask.title!, color: getColorForStatus(subTask.status))
                                                }
                                                else
                                                {
                                                    SubtaskCardView(title: subTask.title!,
                                                                    hours: subTask.estimatedHours ?? 0,
                                                                    status: subTask.status ?? "New",
                                                                    hasNotes: subTask.note != nil && subTask.note != "",
                                                                    color: getColorForStatus(subTask.status))
                                                    .padding(4)
                                                    .shadow(radius: 6)
                                                }
                                            }
                                        }
                                    }
                                }
                                .frame(width: geometry.size.width * 0.7)
                                .cornerRadius(4)
                                .padding(4)

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
                
                if let totalHours = goal.subTasks?.reduce(0, { $0 + ($1.estimatedHours ?? 0) }), totalHours > 0 {
                    let completedHours = goal.subTasks?.filter({ $0.status?.lowercased() == "completed" }).reduce(0, { $0 + ($1.estimatedHours ?? 0) }) ?? 0
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
        .popover(isPresented: $showingDetail) {
            if let selectedSubTask = selectedSubTask.subTask{
                SubtaskDetailModal(goal: .constant(goal), subTask: .constant(selectedSubTask))
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
}

class SelectedSubTask: ObservableObject {
    @Published var subTask: SubTask? = nil
}
