//
//  AddNewGoalView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/10/24.
//  Copyright © 2024 Brewer. All rights reserved.
//

import Foundation
import SwiftUI

struct AddNewGoalView: View {
    @Environment(\.presentationMode) var presentationMode
    @State private var goal = Goal()
    @State private var title = "New Goal"
    @State private var targetEndDate: Date = Date()+7
    var onSave: (Goal) -> Void

    var body: some View {
        NavigationView {
            Form {
                Section(header: Text("Goal Details")) {
                    TextField("Title", text: $title)
                }

                Section(header: Text("Target End Date")) {
                    DatePicker("Target End Date", selection: $targetEndDate, displayedComponents: .date)
                }
            }
            .navigationBarTitle("Add New Goal", displayMode: .inline)
            .navigationBarItems(
                leading: Button("Cancel") {
                    presentationMode.wrappedValue.dismiss()
                },
                trailing: Button("Save") {
                    var subtask = SubTask()
                    subtask.title = "First Task"
                    subtask.status = "New"
                    subtask.estimatedHours = 1
                    
                    goal.title = title
                    goal.subTasks = [subtask]
                    goal.status = 0
                    onSave(goal)
                    presentationMode.wrappedValue.dismiss()
                }
            )
        }
    }
}
