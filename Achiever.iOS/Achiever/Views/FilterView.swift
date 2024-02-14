//
//  FilterView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/11/24.
//  Copyright © 2024 Brewer. All rights reserved.
//

import Foundation
import SwiftUI

struct FilterView: View {
    @Binding var sortOption: SortOption
    @Binding var hideCompletedGoals: Bool
    @Binding var hidePastGoals: Bool
    @Environment(\.presentationMode) var presentationMode

    var body: some View {
        NavigationView {
              Form {
                Section(header: Text("Goal Display")) {
                    Toggle(isOn: $hideCompletedGoals) {
                        Text("Hide Completed Goals")
                    }

                    Toggle(isOn: $hidePastGoals) {
                        Text("Hide Past Goals")
                    }
                }
            }
            .navigationBarTitle("Filters", displayMode: .inline)
            .navigationBarItems(leading: Button("Close") {
                presentationMode.wrappedValue.dismiss()
            })
            .navigationBarItems(trailing: Button("Reset") {
                hideCompletedGoals = false
                hidePastGoals = false
                presentationMode.wrappedValue.dismiss()
            })
        }
    }
}
