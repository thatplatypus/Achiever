//
//  GoalOverviewLoadingView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/8/24.
//

import Foundation
import SwiftUI

struct LoadingView: View {
    var body: some View {
        VStack {
            ProgressView()
            ForEach(0..<3) { _ in
                SkeletonCard()
            }
        }
    }
}
